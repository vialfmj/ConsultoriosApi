using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistsList
{
    public class GetDentistsListQuery : DentistsFilterDTO, IRequest<PagedDTO<DentistsListDTO>>
    {
    }
}
