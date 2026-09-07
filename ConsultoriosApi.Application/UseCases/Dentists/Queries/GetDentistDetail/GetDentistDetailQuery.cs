using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistDetail
{
    public class GetDentistDetailQuery : IRequest<DentistDetailDTO>
    {
        public Guid Id { get; set; }
    }
}
