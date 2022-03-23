using FilterExceptionIssue.WebApi.Common.Exceptions;
using FilterExceptionIssue.WebApi.Common.Models;
using FluentValidation;
using MassTransit;

namespace FilterExceptionIssue.WebApi.Common.Filters
{
    public class RequestValidationScopedFilter<TMessage> : IFilter<ConsumeContext<TMessage>>
     where TMessage : class
    {
        private readonly IEnumerable<IValidator<TMessage>> validators;

        public RequestValidationScopedFilter(IEnumerable<IValidator<TMessage>> validators)
        {
            this.validators = validators;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("RequestValidationScopedFilter<TMessage> scope");
        }

        public async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
        {
            if (validators.Any())
            {
                var validationContext = new ValidationContext<TMessage>(context.Message);

                var validationErrors = validators
                    .Select(v => v.Validate(validationContext))
                    .SelectMany(validationResult => validationResult.Errors)
                    .Select(e => new ValidationError { Field = e.PropertyName, ErrorMessage = e.ErrorMessage })
                    .ToList();

                if (validationErrors.Count != 0)
                {
                    var data = validationErrors.ToDictionary(k => k.Field, v => v.ErrorMessage);

                    var requestValidationException = new RequestValidationException(validationErrors);

                    //throw requestValidationException;

                    throw new MassTransitApplicationException(requestValidationException, data);
                }
            }

            await next.Send(context);
        }
    }
}
