using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficeDetail
{
    public class GetOfficeDetailUseCase : IRequestHandler<GetOfficeDetailQuery, OfficeDetailDTO>
    {
        private readonly IOfficeRepository repository;

        public GetOfficeDetailUseCase(IOfficeRepository repository)
        {
            this.repository = repository;
        }
        public async Task<OfficeDetailDTO> Handle(GetOfficeDetailQuery request)
        {
            var office = await repository.GetById(request.Id);
            if(office is null)
            {
                throw new NotFoundException();
            }

            var dto = office.ToDto();
            return dto;
        }
    }
}
