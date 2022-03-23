using FilterExceptionIssue.WebApi.Common.Models;

namespace FilterExceptionIssue.WebApi.Common.Exceptions
{
    public class RequestValidationException : Exception
    {
        public RequestValidationException(IList<ValidationError> validationErrors) : base("One or more validation errors have occurred.")
        {
            ValidationErrors = validationErrors ?? new List<ValidationError>();
        }

        public IList<ValidationError> ValidationErrors { get; }
    }
}
