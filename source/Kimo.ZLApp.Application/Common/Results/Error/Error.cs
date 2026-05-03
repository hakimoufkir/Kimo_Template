using System.ComponentModel.DataAnnotations;

namespace Kimo.ZLApp.Application.Common.Results.Error;

public record Error(ErrorType Type, string Message)
{
    public Dictionary<string, object>? Metadata { get; init; }

    public static Error Failure()
    {
        return new Error(ErrorType.Failure, "The request has failed.");
    }

    public static Error Unexpected(Exception ex)
    {
        return new Error(ErrorType.Unexpected, "An unexpected error has occurred.")
        {
            Metadata = new Dictionary<string, object> { { "exception", ex.Message } }
        };
    }

    public static Error NotFound()
    {
        return new Error(ErrorType.NotFound, "The requested item was not found.");
    }

    public static Error Forbidden()
    {
        return new Error(ErrorType.Forbidden, "Access to this resource denied.");
    }

    public static Error Unauthorized()
    {
        return new Error(ErrorType.Unauthorized, "You are not authorized. Please sign in.");
    }

    public static Error Validation(IEnumerable<ValidationResult> validationErrors)
    {
        var metaData = validationErrors
            .SelectMany(err => err.MemberNames.Select(member => new { member, err.ErrorMessage }))
            .ToDictionary(
                x => x.member, object (x) => x.ErrorMessage ?? "Validation error",
                StringComparer.OrdinalIgnoreCase
            );

        return new Error(ErrorType.Validation, "The request was not processed due to validation errors.")
        {
            Metadata = new Dictionary<string, object> { { "validationErrors", metaData } }
        };
    }

    public static Error Conflict(string identifier)
    {
        return new Error(ErrorType.Conflict, "An item with the same key already exists.")
        {
            Metadata = new Dictionary<string, object> { { "conflictingIdentifier", identifier } }
        };
    }

    public static Error DependencyNotFound(string identifier)
    {
        return new Error(ErrorType.DependencyNotFound, "A necessary dependency was not found.")
        {
            Metadata = new Dictionary<string, object> { { "missingIdentifier", identifier } }
        };
    }
}