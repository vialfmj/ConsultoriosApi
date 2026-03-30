using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficeDetail
{
    public class GetOfficeDetailQuery : IRequest<OfficeDetailDTO>
    {
        public Guid Id { get; set; }
    }
}
