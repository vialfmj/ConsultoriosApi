using ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentsList;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.Contracts.Repositories
{
    public interface IAppointmentsRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetFiltered(AppointmentsFilterDTO filter);

        // Devuelve true si existe otro turno en estado Scheduled del mismo dentista
        // o del mismo consultorio que se superpone con el intervalo indicado.
        // excludeAppointmentId se usa para excluir el propio turno al reprogramar.
        Task<bool> HasOverlap(Guid dentistId, Guid officeId, TimeInterval timeInterval, Guid? excludeAppointmentId = null);
    }
}
