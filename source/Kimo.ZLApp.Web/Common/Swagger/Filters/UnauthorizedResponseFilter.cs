using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kimo.ZLApp.Web.Common.Swagger.Filters;

[ExcludeFromCodeCoverage]
internal class UnauthorizedResponseFilter : BaseResponseFilter, IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.ContainsKey("401"))
        {
            return;
        }

        var schema = GetProblemDetailsSchema(context);

        var example = CreateProblemExample(
            "https://kimo-online.de/developers/docs/problems/unauthorized",
            "Unauthorized",
            401,
            "You are not authorized. Please sign in."
        );

        operation.Responses["401"] = new OpenApiResponse
        {
            Description = "Beim Aufruf des Endpunkts wurde kein oder ein ungültiges Token übergeben.",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [MediaTypeNames.Application.ProblemJson] = new() { Schema = schema, Example = example }
            }
        };
    }
}