using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Offices.Commands.DeleteOffice
{
    public class DeleteOfficeCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}
