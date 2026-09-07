using ConsultoriosApi.Api.DTOS.Offices;
using ConsultoriosApi.Application.UseCases.Offices.Commands.CreateOffice;
using ConsultoriosApi.Application.UseCases.Offices.Commands.DeleteOffice;
using ConsultoriosApi.Application.UseCases.Offices.Commands.UpdateOffice;
using ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficeDetail;
using ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficesList;
using ConsultoriosApi.Application.Utils.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace ConsultoriosApi.Api.Controllers
{
    [ApiController]
    [Route("api/offices")]
    public class OfficesController : ControllerBase
    {
        private readonly IMediator mediator;

        public OfficesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateOfficeDto createOfficeDto)
        {
            var command = new CreateOfficeCommand { Name = createOfficeDto.Name };
            await mediator.Send(command);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateOfficeDto updateOfficeDto)
        {
            var command = new UpdateOfficeCommand { Id = id, Name = updateOfficeDto.Name };
            await mediator.Send(command);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteOfficeCommand { Id = id };
            await mediator.Send(command);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OfficeDetailDTO>> Get(Guid id)
        {
            var query = new GetOfficeDetailQuery { Id= id };
            var result = await mediator.Send(query);
            return result;
        }
        [HttpGet]
        public async Task<ActionResult<List<OfficesListDTO>>> Get()
        {
            var query = new GetOfficesListQuery();
            var result = await mediator.Send(query);
            return result;
        }
    }
}
