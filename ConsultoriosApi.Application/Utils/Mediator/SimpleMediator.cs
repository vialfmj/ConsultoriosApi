using ConsultoriosApi.Application.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.Utils.Mediator
{
    public class SimpleMediator : IMediator
    {
        private readonly IServiceProvider serviceProvider;

        public SimpleMediator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
            var validator = serviceProvider.GetService(validatorType);

            if (validator is not null)
            {
                var validatorMethod = validatorType.GetMethod("ValidateAsync")!;

                var validateTask =  (Task)validatorMethod!.Invoke(validator, new object[] { request, CancellationToken.None })!;

                await validateTask.ConfigureAwait(false);

                var validationResultProperty = validateTask.GetType().GetProperty("Result")!;
                var validationResult = (ValidationResult)validationResultProperty!.GetValue(validateTask)!;


                if (!validationResult.IsValid)
                {
                    throw new Exceptions.ValidationException(validationResult);
                }
            }

            var useCaseType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

            var useCase = serviceProvider.GetService(useCaseType);
            
            if(useCase is null)
            {
                throw new MediatorException($"Does not exist a handler for {request.GetType().Name}");
            }
            var method = useCaseType.GetMethod("Handle")!;

            return await (Task<TResponse>)method.Invoke(useCase, new object[] { request })!;
        }
    }
}
