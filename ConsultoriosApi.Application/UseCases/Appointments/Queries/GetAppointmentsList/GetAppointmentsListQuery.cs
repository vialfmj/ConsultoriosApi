
using ConsultoriosApi.Application.Contracts.Repositories.Models;
using ConsultoriosApi.Application.Utils.Mediator;

namespace ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentsList
{
    public class GetAppointmentsListQuery : AppointmentsFilterDTO, IRequest<PagedDTO<AppointmentsListDTO>>
    {
    }
}
