using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Dentists.Commands.DeleteDentist
{
    public class DeleteDentistCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}
