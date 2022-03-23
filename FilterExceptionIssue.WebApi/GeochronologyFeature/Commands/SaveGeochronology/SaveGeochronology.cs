using FilterExceptionIssue.WebApi.Common.Models;
using FluentValidation;

namespace FilterExceptionIssue.WebApi.GeochronologyFeature.Commands.SaveGeochronology
{
    public interface SaveGeochronology
    {
        public Geochronology Geochronology { get; }

        public class Validator : AbstractValidator<SaveGeochronology>
        {
            public Validator()
            {
                RuleFor(v => v.Geochronology.Title)
                    .NotEmpty().WithMessage("Title is required.")
                    .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
                    .OverridePropertyName("Title");
            }
        }
    }
}
