using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentDetail
{
    public class GetAppointmentDetailQuery : IRequest<AppointmentDetailDTO>
    {
        public Guid Id { get; set; }
    }
}
