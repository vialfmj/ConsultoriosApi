using ConsultoriosApi.Dominio.Entities;

namespace ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistDetail
{
    public static class MapperExtensions
    {
        public static DentistDetailDTO ToDto(this Dentist dentist)
        {
            var dto = new DentistDetailDTO
            {
                Id = dentist.Id,
                Name = dentist.Name,
                Email = dentist.Email.Valor
            };
            return dto;
        }
    }
}
