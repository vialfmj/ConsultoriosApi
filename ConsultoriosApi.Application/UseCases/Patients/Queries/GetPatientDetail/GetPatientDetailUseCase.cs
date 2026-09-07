using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientDetail
{
    public class GetPatientDetailUseCase : IRequestHandler<GetPatientDetailQuery, PatientDetailDTO>
    {
        private readonly IPatientsRepository repository;

        public GetPatientDetailUseCase(IPatientsRepository repository)
        {
            this.repository = repository;
        }
        public async Task<PatientDetailDTO> Handle(GetPatientDetailQuery request)
        {
            var patient = await repository.GetById(request.Id);
            if (patient is null)
            {
                throw new NotFoundException();
            }

            var dto = patient.ToDto();
            return dto;
        }
    }
}
