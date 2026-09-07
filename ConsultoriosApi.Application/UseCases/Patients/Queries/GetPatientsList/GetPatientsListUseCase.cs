using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientsList
{
    public class GetPatientsListUseCase : IRequestHandler<GetPatientsListQuery, PagedDTO<PatientsListDTO>>
    {
        private readonly IPatientsRepository repository;

        public GetPatientsListUseCase(IPatientsRepository repository)
        {
            this.repository = repository;
        }
        public async Task<PagedDTO<PatientsListDTO>> Handle(GetPatientsListQuery request)
        {
            var patients = await repository.GetFiltered(request);
            var patientsTotal = await repository.GetTotalRecordCount();
            var patientsDto = patients.Select(patient => patient.ToDto()).ToList();

            var pagedDTO = new PagedDTO<PatientsListDTO>
            {
                Elements = patientsDto,
                Total = patientsTotal
            };

            return pagedDTO;
        }
    }
}
