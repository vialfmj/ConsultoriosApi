using ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistsList;
using ConsultoriosApi.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.Contracts.Repositories
{
    public interface IDentistsRepository : IRepository<Dentist>
    {
        Task<IEnumerable<Dentist>> GetFiltered(DentistsFilterDTO filter);
    }
}
