using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Patients.Commands.UpdatePatient
{
    public class UpdatePatientCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
