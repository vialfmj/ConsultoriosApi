using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Patients.Commands.CreatePatient
{
    public class CreatePatientCommand : IRequest<Guid>
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
