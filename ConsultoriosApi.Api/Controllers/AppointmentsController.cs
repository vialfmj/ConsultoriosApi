using ConsultoriosApi.Api.DTOS.Appointments;
using ConsultoriosApi.Api.Utils;
using ConsultoriosApi.Application.UseCases.Appointments.Commands.CancelAppointment;
using ConsultoriosApi.Application.UseCases.Appointments.Commands.CompleteAppointment;
using ConsultoriosApi.Application.UseCases.Appointments.Commands.CreateAppointment;
using ConsultoriosApi.Application.UseCases.Appointments.Commands.RescheduleAppointment;
using ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentDetail;
using ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentsList;
using ConsultoriosApi.Application.Utils.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace ConsultoriosApi.Api.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AppointmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateAppointmentDto createAppointmentDto)
        {
            var command = new CreateAppointmentCommand
            {
                PatientId = createAppointmentDto.PatientId,
                DentistId = createAppointmentDto.DentistId,
                OfficeId = createAppointmentDto.OfficeId,
                Start = createAppointmentDto.Start,
                End = createAppointmentDto.End
            };
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, RescheduleAppointmentDto rescheduleAppointmentDto)
        {
            var command = new RescheduleAppointmentCommand
            {
                Id = id,
                Start = rescheduleAppointmentDto.Start,
                End = rescheduleAppointmentDto.End
            };
            await mediator.Send(command);
            return Ok();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var command = new CancelAppointmentCommand { Id = id };
            await mediator.Send(command);
            return Ok();
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var command = new CompleteAppointmentCommand { Id = id };
            await mediator.Send(command);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDetailDTO>> GetById(Guid id)
        {
            var query = new GetAppointmentDetailQuery { Id = id };
            var result = await mediator.Send(query);
            return result;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppointmentsListDTO>>> Get([FromQuery] GetAppointmentsListQuery query)
        {
            var result = await mediator.Send(query);
            HttpContext.InsertPagingInHeader(result.Total);

            return result.Elements;
        }
    }
}
