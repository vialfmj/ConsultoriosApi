using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Commands.UpdateOffice
{
    public class UpdateOfficeCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }   
    }
}
