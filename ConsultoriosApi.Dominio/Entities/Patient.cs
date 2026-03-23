using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultoriosApi.Dominio.Exceptions;
using System.Xml.Linq;
using ConsultoriosApi.Dominio.ValueObjects;

namespace ConsultoriosApi.Dominio.Entities
{
    public class Patient
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;

        public Patient(string name, Email email)
        {
            if (string.IsNullOrEmpty(name)) throw new BusinessRuleException($"{nameof(name)} is required");
            if (email is null) throw new BusinessRuleException($"{nameof(email)} is required");

            Id = Guid.CreateVersion7();
            Name = name;
            Email = email;
        }
    }
    
}
