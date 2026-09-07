using ConsultoriosApi.Api.DTOS.Dentists;
using ConsultoriosApi.Api.Utils;
using ConsultoriosApi.Application.UseCases.Dentists.Commands.CreateDentist;
using ConsultoriosApi.Application.UseCases.Dentists.Commands.DeleteDentist;
using ConsultoriosApi.Application.UseCases.Dentists.Commands.UpdateDentist;
using ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistDetail;
using ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistsList;
using ConsultoriosApi.Application.Utils.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace ConsultoriosApi.Api.Controllers
{
    [ApiController]
    [Route("api/dentists")]
    public class DentistsController : ControllerBase
    {
        private readonly IMediator mediator;

        public DentistsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateDentistDto createDentistDto)
        {
            var command = new CreateDentistCommand { Name = createDentistDto.Name, Email = createDentistDto.Email };
            await mediator.Send(command);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateDentistDto updateDentistDto)
        {
            var command = new UpdateDentistCommand { Id = id, Name = updateDentistDto.Name, Email = updateDentistDto.Email };
            await mediator.Send(command);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteDentistCommand { Id = id };
            await mediator.Send(command);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DentistDetailDTO>> GetById(Guid id)
        {
            var query = new GetDentistDetailQuery { Id = id };
            var result = await mediator.Send(query);
            return result;
        }
        [HttpGet]
        public async Task<ActionResult<List<DentistsListDTO>>> Get([FromQuery] GetDentistsListQuery query)
        {
            var result = await mediator.Send(query);
            HttpContext.InsertPagingInHeader(result.Total);

            return result.Elements;
        }
    }
}
