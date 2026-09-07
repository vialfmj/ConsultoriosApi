using ConsultoriosApi.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistsList
{
    public static class MapperExtensions
    {
        public static DentistsListDTO ToDto(this Dentist dentist)
        {
            var dto = new DentistsListDTO { Id = dentist.Id, Name = dentist.Name, Email = dentist.Email.Valor };
            return dto;
        }
    }
}
