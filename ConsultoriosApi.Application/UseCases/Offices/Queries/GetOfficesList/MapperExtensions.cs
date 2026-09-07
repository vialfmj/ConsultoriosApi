using ConsultoriosApi.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficesList
{
    public static class MapperExtensions
    {
        public static OfficesListDTO ToDto(this Office office)
        {
            var dto = new OfficesListDTO { Id = office.Id, Name = office.Name };
            return dto;
        }
    }
}
