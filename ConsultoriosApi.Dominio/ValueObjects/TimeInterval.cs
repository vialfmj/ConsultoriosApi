using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultoriosApi.Dominio.Exceptions;

namespace ConsultoriosApi.Dominio.ValueObjects
{
    /// <summary>Start y End se asumen siempre en UTC.</summary>
    public record TimeInterval
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        private TimeInterval() { }
        public TimeInterval(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new BusinessRuleException("The start date cannot be after the end date.");
            }
            this.Start = start;
            this.End = end;
        }
    }
}
