# Plan: implementar `Appointment` (turno)

## Contexto

`Appointment` representa un turno que vincula un `Patient`, un `Dentist` y un `Office` en un rango horario (`TimeInterval`), con un estado (`DateState`). La entidad de dominio **ya existe** (`ConsultoriosApi.Dominio/Entities/Appointment.cs`), pero falta implementarla en Application, Persistence y Api.

Esto estaba bloqueado porque `Dentist` no tenía persistencia (sin DbSet, sin migración) — eso ya se resolvió (ver historial: CRUD completo de Dentist en las 4 capas, migración `AddDentistsTable` aplicada). Con `Patient`, `Dentist` y `Office` ya completos en las 4 capas, `Appointment` puede implementarse con FKs reales.

## Estado actual de `Appointment` en Dominio

```csharp
public class Appointment
{
    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public Guid DentistId { get; private set; }
    public Guid OfficeId { get; private set; }
    public TimeInterval TimeInterval { get; private set; }
    public DateState State { get; private set; }
    public Patient? Patient { get; private set; }
    public Dentist? Dentist { get; private set; }
    public Office? Office { get; private set; }

    public Appointment(Guid patientId, Guid dentistId, Guid officeId, TimeInterval timeInterval) { ... }
    public void Cancel() { ... }   // exige State == Scheduled
    public void Complete() { ... } // exige State == Scheduled
}
```

`DateState` (enum): `Scheduled = 1, Canceled = 2, Completed = 3`.
`TimeInterval` (value object): `Start`, `End` (`DateTime`), valida `Start <= End`.

### Bugs a corregir antes de avanzar

1. **`Appointment.Complete()`** (línea 51) setea `State = DateState.Canceled;` en vez de `DateState.Completed`. Bug claro, hay que corregirlo.
2. **`AppointmentTests.Complete_ChangesStateToCompleted_WhenAppointmentIsScheduled`** actualmente assertea `DateState.Canceled` — está validando el bug en lugar del comportamiento correcto. Se corrige junto con el punto anterior.
3. `Appointment` no tiene constructor privado sin parámetros (`private Appointment() {}`) como sí tiene `Patient`/`Dentist` — hace falta para que EF Core materialice la entidad.
4. `TimeInterval` (value object) tampoco tiene constructor privado sin parámetros, a diferencia de `Email`. Si se mapea como `ComplexProperty` en EF (como `Email` en `PatientSettings`/`DentistSettings`), probablemente EF también lo necesite ahí — verificar al escribir `AppointmentSettings` y agregarlo si hace falta.

## Decisiones de diseño (confirmadas con el usuario)

- **Alcance de `UpdateAppointment`**: solo reprograma el `TimeInterval` (`Reschedule`). Cambiar paciente/dentista/consultorio implica cancelar y crear un turno nuevo.
- **`DeleteAppointment`**: no se expone borrado físico. Solo `Cancel` (estado), para preservar el historial.
- **Validación de solapamiento**: SÍ se valida. No se debe poder crear/reprogramar un turno si el mismo `Dentist` u `Office` ya tiene otro turno `Scheduled` que se superpone en el tiempo. Se agrega como regla de negocio nueva en el UseCase, apoyada en `IAppointmentsRepository.HasOverlap(...)`.
- **Filtros de `GetAppointmentsList`**: `PatientId`, `DentistId`, `OfficeId`, `State` y rango de fechas (`From`/`To` sobre `TimeInterval.Start`).

## Cambios a realizar

### 1. Dominio (`ConsultoriosApi.Dominio/Entities/Appointment.cs`)
- Corregir `Complete()` → `State = DateState.Completed;`.
- Agregar `private Appointment() { }`.
- Agregar método `Reschedule(TimeInterval nuevoIntervalo)` que solo permita reprogramar cuando `State == Scheduled` (reutilizando la validación de fecha del constructor).
- Revisar/agregar constructor privado sin parámetros en `ValueObjects/TimeInterval.cs` si EF lo requiere para el `ComplexProperty`.
- Corregir el test `AppointmentTests.cs` que valida el bug de `Complete()`.

### 2. Application (`ConsultoriosApi.Application/UseCases/Appointments/`)
Mismo patrón que `Patients`/`Dentists`:
- `Contracts/Repositories/IAppointmentsRepository.cs`: `IRepository<Appointment>` + `GetFiltered(AppointmentsFilterDTO filter)` + `HasOverlap(dentistId, officeId, timeInterval, excludeAppointmentId)`.
- `Commands/CreateAppointment/`: `CreateAppointmentCommand` (`PatientId`, `DentistId`, `OfficeId`, `Start`, `End`), validator (fechas no vacías, `Start < End`, `Start >= UtcNow` ya lo valida el dominio), use case (verificar `HasOverlap` → `ConflictException` si superpone, `new Appointment(...)`, `Add` + `Commit`/`RollBack`).
- `Commands/RescheduleAppointment/`: reprograma horario, valida `HasOverlap` excluyendo el propio turno antes de `appointment.Reschedule(...)`.
- `Commands/CancelAppointment/`: `CancelAppointmentCommand { Id }`, use case (`GetById` → `NotFoundException` → `appointment.Cancel()` → `Update` + `Commit`/`RollBack`).
- `Commands/CompleteAppointment/`: idem, llamando `appointment.Complete()`.
- `Queries/GetAppointmentDetail/`: query + use case + `MapperExtensions.ToDto` + `AppointmentDetailDTO` (incluir datos útiles del turno: `PatientId`, `DentistId`, `OfficeId`, `Start`, `End`, `State`; evaluar si conviene incluir `PatientName`/`DentistName` vía navegación).
- `Queries/GetAppointmentsList/`: `AppointmentsFilterDTO` (paginación + filtros a definir) + query + use case + mapper + `AppointmentsListDTO`.
- Registrar los handlers en `ApplicationServiceRegistry.cs`.

### 3. Persistence
- `ConsultoriosApiDbContext.cs`: agregar `DbSet<Appointment> Appointments { get; set; }`.
- `Settings/AppointmentSettings.cs`: `IEntityTypeConfiguration<Appointment>`.
  - `TimeInterval` como `ComplexProperty` (`Start`/`End` → columnas `Start`/`End`, tipo `datetime2`).
  - FKs explícitas: `HasOne(a => a.Patient).WithMany().HasForeignKey(a => a.PatientId)`, ídem para `Dentist` y `Office`. **Primer caso de relaciones FK en el proyecto** — no hay precedente, revisar con cuidado (delete behavior: probablemente `Restrict` para no borrar turnos en cascada al borrar un paciente/dentista/consultorio).
- `Repositories/AppointmentsRepository.cs`: `Repository<Appointment>, IAppointmentsRepository`, con `GetFiltered` (filtros a definir) + `Paginate`.
- `PersistenceServiceRegistry.cs`: registrar `IAppointmentsRepository`.
- Migración: `dotnet ef migrations add AddAppointmentsTable --project ConsultoriosApi.Persistence --startup-project ConsultoriosApi.Api`. Revisar el archivo generado (columnas, FKs, índices) antes de aplicar. **No aplicar sin confirmación explícita.**

### 4. Api
- `DTOS/Appointments/CreateAppointmentDto.cs`, `RescheduleAppointmentDto.cs` — DataAnnotations reflejando el validator de Application.
- `Controllers/AppointmentsController.cs`: `[Route("api/appointments")]`. Endpoints: `POST` (crear), `PUT {id}` (reprogramar), `POST {id}/cancel`, `POST {id}/complete`, `GET {id}`, `GET` (lista paginada con filtros). Sin try/catch (middleware global ya maneja `NotFoundException`/`ValidationException`).
- `Appointments.http`: mismo formato que `Dentists.http`/`Patients.http`, con ejemplos de cada endpoint incluyendo `Cancel`/`Complete`.

### 5. Tests
- `ConsultorioApiTests/Dominio/Entities/AppointmentTests.cs`: corregir el test del bug de `Complete()`, agregar casos para el nuevo método de reprogramación si se agrega.
- `ConsultorioApiTests/Application/UseCases/Appointments/`: un archivo por caso de uso (Create, Reschedule, Cancel, Complete, GetDetail, GetList), MSTest + NSubstitute, siguiendo el patrón de `ConsultorioApiTests/Application/UseCases/Dentists/*`.

## Verificación
1. `dotnet build` sobre toda la solución.
2. `dotnet test` — confirmar que pasan los tests nuevos y existentes.
3. Revisar el archivo de migración generado antes de aplicarlo (no aplicar sin confirmación).
4. Probar manualmente con `Appointments.http` (crear turno, reprogramar, cancelar, completar, listar con filtros, obtener por id) contra la API corriendo.

## Próximo paso sugerido
Decisiones de diseño ya resueltas. Implementar en orden de dependencia: Dominio → Application → Persistence → Api → Tests, delegando por capa a los subagentes correspondientes (`dominio-layer`, `application-layer`, `persistence-layer`, `api-layer`, `test-layer`).
