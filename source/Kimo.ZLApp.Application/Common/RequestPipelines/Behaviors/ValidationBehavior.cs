using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common.Results.Error;

namespace Kimo.ZLApp.Application.Common.RequestPipelines.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    ILogger<ValidationBehavior<TRequest, TResponse>> logger,
    IServiceProvider serviceProvider
)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(request, serviceProvider, null);

        var isValid = Validator.TryValidateObject(
            request,
            context,
            validationResults,
            true
        );

        if (!isValid)
        {
            logger.ValidationFailed(typeof(TRequest).Name, validationResults.Count);

            var error = Error.Validation(validationResults);
            return BehaviorResult.Fail<TResponse>(error);
        }

        logger.ValidationPassed(typeof(TRequest).Name);

        return await next();
    }
}