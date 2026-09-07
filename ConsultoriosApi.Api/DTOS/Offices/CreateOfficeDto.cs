using System.ComponentModel.DataAnnotations;

namespace ConsultoriosApi.Api.DTOS.Offices
{
    public class CreateOfficeDto
    {
        [Required]
        [StringLength(150)]
        public required string Name { get; set; }
    }
}
