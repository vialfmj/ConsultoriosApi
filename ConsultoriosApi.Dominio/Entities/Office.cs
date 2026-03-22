using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultoriosApi.Dominio.Exceptions;

namespace ConsultoriosApi.Dominio.Entities
{
    public class Office
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Office(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new BusinessRuleException($"{nameof(name)} is required.");
            }

            Id = Guid.CreateVersion7();
            Name = name;
        }
    }
}
