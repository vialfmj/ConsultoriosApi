using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistsList;
using ConsultoriosApi.Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Persistence.Repositories
{
    public class DentistsRepository : Repository<Dentist>, IDentistsRepository
    {
        private readonly ConsultoriosApiDbContext context;
        public DentistsRepository(ConsultoriosApiDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Dentist>> GetFiltered(DentistsFilterDTO filter)
        {
            var queryable = context.Dentists.AsQueryable();

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
