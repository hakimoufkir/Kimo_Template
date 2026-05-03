using System.ComponentModel.DataAnnotations;

namespace Kimo.ZLApp.Application.WeatherForecasts.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class IsEvenAttribute() : ValidationAttribute("The field {0} must be even.")
{
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        var propertyName = context.MemberName ?? string.Empty;

        if (value is not int number)
        {
            return new ValidationResult(FormatErrorMessage(propertyName), [propertyName]);
        }

        return (number & 1) != 0
            ? new ValidationResult(FormatErrorMessage(propertyName), [propertyName])
            : ValidationResult.Success;
    }
}