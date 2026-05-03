using System.ComponentModel.DataAnnotations;
using Kimo.ZLApp.Application.Common.Filtering.Requests;

namespace Kimo.ZLApp.Application.Common.Filtering.Validators;

[AttributeUsage(AttributeTargets.Property)]
public sealed class AllowedFilterFieldsAttribute(params string[] allowedFields)
    : ValidationAttribute("One or more filters contain an invalid field.")
{
    private readonly HashSet<string> _allowed = new(
        allowedFields.Select(f => f.Trim()),
        StringComparer.OrdinalIgnoreCase);

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is not FilterOptions filterOptions)
        {
            return ValidationResult.Success;
        }

        foreach (var filter in filterOptions.Filters)
        {
            var normalizedField = filter.Field.Trim();
            if (!_allowed.Contains(normalizedField))
            {
                return new ValidationResult(
                    $"'{filter.Field}' is not an allowed filter field. Allowed: {string.Join(", ", _allowed)}",
                    [context.MemberName ?? "Filters"]
                );
            }
        }

        return ValidationResult.Success;
    }
}