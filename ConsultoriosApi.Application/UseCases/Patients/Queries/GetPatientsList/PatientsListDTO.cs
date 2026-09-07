using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientsList
{
    public class PatientsListDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
