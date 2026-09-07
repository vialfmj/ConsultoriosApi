using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficesList
{
    public class GetOfficesListUseCase : IRequestHandler<GetOfficesListQuery, List<OfficesListDTO>>
    {
        private readonly IOfficesRepository repository;

        public GetOfficesListUseCase(IOfficesRepository repository)
        {
            this.repository = repository;
        }
        public async Task<List<OfficesListDTO>> Handle(GetOfficesListQuery request)
        {

            var offices = await repository.GetAll();
            var officesDto = offices.Select(office => office.ToDto()).ToList();

            return officesDto;
        }
    }
}
