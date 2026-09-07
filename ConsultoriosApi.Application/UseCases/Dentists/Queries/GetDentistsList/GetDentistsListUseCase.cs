using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistsList
{
    public class GetDentistsListUseCase : IRequestHandler<GetDentistsListQuery, PagedDTO<DentistsListDTO>>
    {
        private readonly IDentistsRepository repository;

        public GetDentistsListUseCase(IDentistsRepository repository)
        {
            this.repository = repository;
        }
        public async Task<PagedDTO<DentistsListDTO>> Handle(GetDentistsListQuery request)
        {
            var dentists = await repository.GetFiltered(request);
            var dentistsTotal = await repository.GetTotalRecordCount();
            var dentistsDto = dentists.Select(dentist => dentist.ToDto()).ToList();

            var pagedDTO = new PagedDTO<DentistsListDTO>
            {
                Elements = dentistsDto,
                Total = dentistsTotal
            };

            return pagedDTO;
        }
    }
}
