using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ConsultoriosApi.Dominio.Exceptions;
using ConsultoriosApi.Dominio.ValueObjects;

namespace ConsultoriosApi.Dominio.Entities
{
    public class Dentist
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;

        private Dentist() { }
        public Dentist(string name, Email email)
        {
            ApplyBusinessRulesName(name);
            ApplyBusinessRulesEmail(email);

            Id = Guid.CreateVersion7();
            Name = name;
            Email = email;
        }
        public void UpdateName(string name)
        {
            ApplyBusinessRulesName(name);
            Name = name;
        }
        public void UpdateEmail(Email email)
        {
            ApplyBusinessRulesEmail(email);
            Email = email;
        }
        private void ApplyBusinessRulesName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new BusinessRuleException($"{nameof(name)} is required");
        }
        private void ApplyBusinessRulesEmail(Email email)
        {
            if (email is null) throw new BusinessRuleException($"{nameof(email)} is required");
        }
    }
}
