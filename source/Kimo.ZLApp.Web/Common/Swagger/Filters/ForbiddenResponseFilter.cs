using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kimo.ZLApp.Web.Common.Swagger.Filters;

[ExcludeFromCodeCoverage]
internal class ForbiddenResponseFilter : BaseResponseFilter, IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.ContainsKey("403"))
        {
            return;
        }

        var schema = GetProblemDetailsSchema(context);

        var example = CreateProblemExample(
            "https://kimo-online.de/developers/docs/problems/forbidden",
            "Forbidden",
            403,
            "Access to this resource denied."
        );

        operation.Responses["403"] = new OpenApiResponse
        {
            Description = "Der Zugriff auf den Endpunkt ist mit dem übergebenen Token nicht erlaubt.",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [MediaTypeNames.Application.ProblemJson] = new() { Schema = schema, Example = example }
            }
        };
    }
}