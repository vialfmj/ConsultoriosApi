using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Utils.Mediator;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentsList
{
    public class GetAppointmentsListUseCase : IRequestHandler<GetAppointmentsListQuery, PagedDTO<AppointmentsListDTO>>
    {
        private readonly IAppointmentsRepository repository;

        public GetAppointmentsListUseCase(IAppointmentsRepository repository)
        {
            this.repository = repository;
        }
        public async Task<PagedDTO<AppointmentsListDTO>> Handle(GetAppointmentsListQuery request)
        {
            var appointments = await repository.GetFiltered(request);
            var appointmentsTotal = await repository.GetTotalRecordCount();
            var appointmentsDto = appointments.Select(appointment => appointment.ToDto()).ToList();

            var pagedDTO = new PagedDTO<AppointmentsListDTO>
            {
                Elements = appointmentsDto,
                Total = appointmentsTotal
            };

            return pagedDTO;
        }
    }
}
