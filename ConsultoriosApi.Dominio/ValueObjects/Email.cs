using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultoriosApi.Dominio.Exceptions;

namespace ConsultoriosApi.Dominio.ValueObjects
{
    public record Email
    {
        public string Valor { get; } = null!;
        public Email(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new BusinessRuleException($"{nameof(email)} is required");
            if (!email.Contains("@")) throw new BusinessRuleException($"{email} is not valid.");

            Valor = email;
        }
    }
}
