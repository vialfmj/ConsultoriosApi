using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistDetail
{
    public class GetDentistDetailUseCase : IRequestHandler<GetDentistDetailQuery, DentistDetailDTO>
    {
        private readonly IDentistsRepository repository;

        public GetDentistDetailUseCase(IDentistsRepository repository)
        {
            this.repository = repository;
        }
        public async Task<DentistDetailDTO> Handle(GetDentistDetailQuery request)
        {
            var dentist = await repository.GetById(request.Id);
            if (dentist is null)
            {
                throw new NotFoundException();
            }

            var dto = dentist.ToDto();
            return dto;
        }
    }
}
