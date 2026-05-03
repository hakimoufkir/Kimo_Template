using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kimo.ZLApp.Web.Common.Swagger.Filters;

[ExcludeFromCodeCoverage]
internal class NotFoundResponseFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.TryGetValue("404", out var notFound))
        {
            if (notFound.Description == "Not Found")
            {
                notFound.Description = "Es konnte kein Element mit der entsprechenden ID gefunden werden.";
            }

            // In your use case, the 404 has no body
            notFound.Content.Clear();
        }
    }
}