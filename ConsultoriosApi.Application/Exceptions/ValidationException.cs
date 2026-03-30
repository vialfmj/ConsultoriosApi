using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.Exceptions
{
    public class ValidationException : Exception
    {
        List<string> ValidationErrors { get; set; } = [];
        public ValidationException(ValidationResult validationResult) 
        {
            foreach(var validationError in validationResult.Errors)
            {
                ValidationErrors.Add(validationError.ErrorMessage);
            }
        }

    }
}
