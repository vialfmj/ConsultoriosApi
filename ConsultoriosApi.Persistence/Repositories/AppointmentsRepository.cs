using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentsList;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.Enums;
using ConsultoriosApi.Dominio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultoriosApi.Persistence.Repositories
{
    public class AppointmentsRepository : Repository<Appointment>, IAppointmentsRepository
    {
        private readonly ConsultoriosApiDbContext context;
        public AppointmentsRepository(ConsultoriosApiDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Appointment>> GetFiltered(AppointmentsFilterDTO filter)
        {
            var queryable = context.Appointments.AsQueryable();

            if (filter.PatientId.HasValue)
            {
                queryable = queryable.Where(a => a.PatientId == filter.PatientId.Value);
            }
            if (filter.DentistId.HasValue)
            {
                queryable = queryable.Where(a => a.DentistId == filter.DentistId.Value);
            }
            if (filter.OfficeId.HasValue)
            {
                queryable = queryable.Where(a => a.OfficeId == filter.OfficeId.Value);
            }
            if (filter.State.HasValue)
            {
                queryable = queryable.Where(a => a.State == filter.State.Value);
            }
            if (filter.From.HasValue)
            {
                queryable = queryable.Where(a => a.TimeInterval.Start >= filter.From.Value);
            }
            if (filter.To.HasValue)
            {
                queryable = queryable.Where(a => a.TimeInterval.Start <= filter.To.Value);
            }

            return await queryable.OrderBy(a => a.TimeInterval.Start)
            .Paginate(filter.Page, filter.RecordsPerPage)
            .ToListAsync();
        }

        public async Task<bool> HasOverlap(Guid dentistId, Guid officeId, TimeInterval timeInterval, Guid? excludeAppointmentId = null)
        {
            var queryable = context.Appointments.Where(a =>
                a.State == DateState.Scheduled &&
                (a.DentistId == dentistId || a.OfficeId == officeId) &&
                a.TimeInterval.Start < timeInterval.End &&
                a.TimeInterval.End > timeInterval.Start);

            if (excludeAppointmentId.HasValue)
            {
                queryable = queryable.Where(a => a.Id != excludeAppointmentId.Value);
            }

            return await queryable.AnyAsync();
        }
    }
}
