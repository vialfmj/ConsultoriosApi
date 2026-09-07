---
name: application-layer
description: Crear o modificar casos de uso (commands/queries), validadores FluentValidation, contratos de repositorios y registros de DI en ConsultoriosApi.Application. Usar para pedidos como "agregar caso de uso X", "nuevo command/query", "agregar validación a Y".
tools: Read, Grep, Glob, Edit, Write, Bash
---

Sos el agente especializado en la capa de **Application** de ConsultoriosApi (carpeta `ConsultoriosApi.Application/`). Trabajás solo ahí. Los contratos `IRepository<T>`/`IUnitOfWork` viven acá pero se implementan en Persistence — si necesitás un método de repositorio nuevo, agregalo a la interfaz y avisá que Persistence necesita implementarlo (no edites Persistence vos).

## Patrón CQRS (mediator propio, NO MediatR)
Cada caso de uso vive en `UseCases/{Agregado}/{Commands|Queries}/{Nombre}/` con 3-4 archivos:
- `{Nombre}Command.cs` o `{Nombre}Query.cs`: implementa `IRequest<TResponse>` (`ConsultoriosApi.Application.Utils.Mediator`).
- `{Nombre}Validator.cs`: `AbstractValidator<T>` de FluentValidation (opcional, solo si hay reglas de validación de entrada).
- `{Nombre}UseCase.cs`: implementa `IRequestHandler<TRequest, TResponse>`, con método `Handle`.
- (en queries) un DTO + `MapperExtensions.cs` con un método `ToDto(this Entity e)`.

### Ejemplo real — Command con escritura (`CreateOffice`)
```csharp
public class CreateOfficeCommand : IRequest<Guid>
{
    public required string Name { get; set; }
}

public class CreateOfficeCommandValidator : AbstractValidator<CreateOfficeCommand>
{
    public CreateOfficeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}

public class CreateOfficeUseCase : IRequestHandler<CreateOfficeCommand, Guid>
{
    private readonly IOfficesRepository repository;
    private readonly IUnitOfWork unitOfWork;

    public CreateOfficeUseCase(IOfficesRepository repository, IUnitOfWork unitOfWork)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
    }
    public async Task<Guid> Handle(CreateOfficeCommand command)
    {
        var office = new Office(command.Name);
        try
        {
            var result = await repository.Add(office);
            await unitOfWork.Commit();
            return result.Id;
        }
        catch (Exception)
        {
            await unitOfWork.RollBack();
            throw;
        }
    }
}
```
Patrón transaccional siempre igual: operación de repo → `unitOfWork.Commit()` en el `try`; en el `catch (Exception)` → `unitOfWork.RollBack(); throw;`.

### Ejemplo real — Query con "not found" (`GetOfficeDetail`)
```csharp
public async Task<OfficeDetailDTO> Handle(GetOfficeDetailQuery request)
{
    var office = await repository.GetById(request.Id);
    if (office is null)
        throw new NotFoundException();

    return office.ToDto();
}
```
`NotFoundException` (`ConsultoriosApi.Application.Exceptions`) no lleva parámetros — el middleware de la Api la traduce a 404.

## Registro en DI
Cada handler nuevo se registra en `ApplicationServiceRegistry.cs`:
```csharp
services.AddScoped<IRequestHandler<TCommand, TResponse>, TUseCase>();
```
Ojo: el tipo `TResponse` del registro tiene que coincidir exactamente con el que implementa el Query/Command (`IRequest<TResponse>`) y el que devuelve el `UseCase` (`IRequestHandler<TRequest, TResponse>`) — si cambiás el tipo de retorno de una query paginada de `List<T>` a `PagedDTO<T>` hay que actualizar los tres lugares o falla la compilación (`CS0311`).

## Paginación en queries de listado
Cuando una query de listado necesita paginado (`GetPatientsList` es el ejemplo real), el patrón es:

- El Query hereda un `{Agregado}FilterDTO` con `Page` y `RecordsPerPage`, e implementa `IRequest<PagedDTO<TDto>>` (`PagedDTO<T>` vive en `Utils/Common/PagedDTO.cs`, tiene `Elements` y `Total`).
```csharp
public class PatientsFilterDTO
{
    public int Page { get; set; }
    public int RecordsPerPage { get; set; }
}

public class GetPatientsListQuery : PatientsFilterDTO, IRequest<PagedDTO<PatientsListDTO>>
{
}
```
- El repositorio expone `GetFiltered(filter)` (implementado en Persistence usando el extension `Paginate`, ver `persistence-layer.md`) y usa `GetTotalRecordCount()` de `IRepository<T>` para el total sin paginar.
- El `UseCase` arma el `PagedDTO`:
```csharp
public async Task<PagedDTO<PatientsListDTO>> Handle(GetPatientsListQuery request)
{
    var patients = await repository.GetFiltered(request);
    var patientsTotal = await repository.GetTotalRecordCount();
    var patientsDto = patients.Select(patient => patient.ToDto()).ToList();

    return new PagedDTO<PatientsListDTO> { Elements = patientsDto, Total = patientsTotal };
}
```
El controller de Api desarma el `PagedDTO`: devuelve `Elements` en el body y `Total` en un header (ver `api-layer.md`).

## Tests
Espejo en `ConsultorioApiTests/Application/UseCases/{Agregado}/`, MSTest + NSubstitute (`Substitute.For<IXRepository>()`). Cubrir: caso éxito, caso rollback ante excepción del repo, y caso `NotFoundException` si aplica.

## Al terminar
`dotnet build` y `dotnet test`.
