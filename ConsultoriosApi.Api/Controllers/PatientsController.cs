using ConsultoriosApi.Api.DTOS.Patients;
using ConsultoriosApi.Api.Utils;
using ConsultoriosApi.Application.UseCases.Patients.Commands.CreatePatient;
using ConsultoriosApi.Application.UseCases.Patients.Commands.DeletePatient;
using ConsultoriosApi.Application.UseCases.Patients.Commands.UpdatePatient;
using ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientDetail;
using ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientsList;
using ConsultoriosApi.Application.Utils.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace ConsultoriosApi.Api.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator mediator;

        public PatientsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreatePatientDto createPatientDto)
        {
            var command = new CreatePatientCommand { Name = createPatientDto.Name, Email = createPatientDto.Email };
            await mediator.Send(command);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdatePatientDto updatePatientDto)
        {
            var command = new UpdatePatientCommand { Id = id, Name = updatePatientDto.Name, Email = updatePatientDto.Email };
            await mediator.Send(command);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeletePatientCommand { Id = id };
            await mediator.Send(command);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDetailDTO>> GetById(Guid id)
        {
            var query = new GetPatientDetailQuery { Id = id };
            var result = await mediator.Send(query);
            return result;
        }
        [HttpGet]
        public async Task<ActionResult<List<PatientsListDTO>>> Get([FromQuery] GetPatientsListQuery query)
        {
            var result = await mediator.Send(query);
            HttpContext.InsertPagingInHeader(result.Total);

            return result.Elements;
        }
    }
}
