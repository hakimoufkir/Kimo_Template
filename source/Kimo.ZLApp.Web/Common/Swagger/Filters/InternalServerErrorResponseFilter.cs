using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kimo.ZLApp.Web.Common.Swagger.Filters;

[ExcludeFromCodeCoverage]
internal class InternalServerErrorResponseFilter : BaseResponseFilter, IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.ContainsKey("500"))
        {
            return;
        }

        var schema = GetProblemDetailsSchema(context);

        var example = CreateProblemExample(
            "https://kimo-online.de/developers/docs/problems/unexpected",
            "Server error",
            500,
            "An unexpected error has occurred.",
            new OpenApiObject { ["exception"] = new OpenApiString("The method or operation is not implemented.") });

        operation.Responses["500"] = new OpenApiResponse
        {
            Description = "Bei der Verarbeitung der Anfrage ist ein interner Serverfehler aufgetreten.",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [MediaTypeNames.Application.ProblemJson] = new() { Schema = schema, Example = example }
            }
        };
    }
}