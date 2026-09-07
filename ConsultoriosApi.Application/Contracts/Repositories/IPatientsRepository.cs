using ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientsList;
using ConsultoriosApi.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.Contracts.Repositories
{
    public interface IPatientsRepository : IRepository<Patient>
    {
        Task<IEnumerable<Patient>> GetFiltered(PatientsFilterDTO filter);
    }
}
