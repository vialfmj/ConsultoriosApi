using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientsList;
using ConsultoriosApi.Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Persistence.Repositories
{
    public class PatientsRepository : Repository<Patient>, IPatientsRepository
    {
        private readonly ConsultoriosApiDbContext context;
        public PatientsRepository(ConsultoriosApiDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Patient>> GetFiltered(PatientsFilterDTO filter)
        {
            var queryable = context.Patients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                queryable = queryable.Where(n => n.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                queryable = queryable.Where(e => e.Email.Valor.Contains(filter.Email));
            }

            return await queryable.OrderBy(x => x.Name)
            .Paginate(filter.Page, filter.RecordsPerPage)
            .ToListAsync();
        }

    }
}
