using ConsultoriosApi.Dominio.Enums;
using System;

namespace ConsultoriosApi.Application.Contracts.Repositories.Models
{
    public class AppointmentsFilterDTO
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;
        public Guid? PatientId { get; set; }
        public Guid? DentistId { get; set; }
        public Guid? OfficeId { get; set; }
        public DateState? State { get; set; }
        /// <summary>Incluir el offset de la zona horaria local (ej: -03:00).</summary>
        public DateTimeOffset? From { get; set; }
        /// <summary>Incluir el offset de la zona horaria local (ej: -03:00).</summary>
        public DateTimeOffset? To { get; set; }
    }
}
