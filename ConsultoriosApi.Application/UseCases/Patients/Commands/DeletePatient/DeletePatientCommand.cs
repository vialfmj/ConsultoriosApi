using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Patients.Commands.DeletePatient
{
    public class DeletePatientCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}
