using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientDetail
{
    public class GetPatientDetailQuery : IRequest<PatientDetailDTO>
    {
        public Guid Id { get; set; }
    }
}
