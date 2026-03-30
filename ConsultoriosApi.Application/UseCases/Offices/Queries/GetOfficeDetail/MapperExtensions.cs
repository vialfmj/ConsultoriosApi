using ConsultoriosApi.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficeDetail
{
    public static class MapperExtensions
    {
        public static OfficeDetailDTO ToDto(this Office office)
        {
            var dto = new OfficeDetailDTO
            {
                Id = office.Id,
                Name = office.Name
            };
            return dto;
        }
    }
}
