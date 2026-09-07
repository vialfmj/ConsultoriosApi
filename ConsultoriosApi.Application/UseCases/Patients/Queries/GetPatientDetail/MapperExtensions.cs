using ConsultoriosApi.Dominio.Entities;

namespace ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientDetail
{
    public static class MapperExtensions
    {
        public static PatientDetailDTO ToDto(this Patient patient)
        {
            var dto = new PatientDetailDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                Email = patient.Email.Valor
            };
            return dto;
        }
    }
}
