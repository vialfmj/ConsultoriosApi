using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficesList
{
    public class OfficesListDTO
    {
        public Guid Id {get; set;}
        public required string Name {get; set;}
    }
}
