using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientsList
{
    public class GetPatientsListQuery : PatientsFilterDTO, IRequest<PagedDTO<PatientsListDTO>>
    {
    }
}
