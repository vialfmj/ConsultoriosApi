using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentDetail
{
    public class GetAppointmentDetailUseCase : IRequestHandler<GetAppointmentDetailQuery, AppointmentDetailDTO>
    {
        private readonly IAppointmentsRepository repository;

        public GetAppointmentDetailUseCase(IAppointmentsRepository repository)
        {
            this.repository = repository;
        }
        public async Task<AppointmentDetailDTO> Handle(GetAppointmentDetailQuery request)
        {
            var appointment = await repository.GetById(request.Id);
            if (appointment is null)
            {
                throw new NotFoundException();
            }

            var dto = appointment.ToDto();
            return dto;
        }
    }
}
