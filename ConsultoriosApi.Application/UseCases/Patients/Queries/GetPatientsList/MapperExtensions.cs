using ConsultoriosApi.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientsList
{
    public static class MapperExtensions
    {
        public static PatientsListDTO ToDto(this Patient patient)
        {
            var dto = new PatientsListDTO { Id = patient.Id, Name = patient.Name, Email = patient.Email.Valor };
            return dto;
        }
    }
}
