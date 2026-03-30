using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficeDetail
{
    public class OfficeDetailDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
