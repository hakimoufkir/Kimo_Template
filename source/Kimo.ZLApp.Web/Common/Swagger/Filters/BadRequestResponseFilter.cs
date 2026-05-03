using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kimo.ZLApp.Web.Common.Swagger.Filters;

[ExcludeFromCodeCoverage]
internal class BadRequestResponseFilter : BaseResponseFilter, IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!operation.Responses.TryGetValue("400", out var badRequest))
        {
            return;
        }

        var schema = GetProblemDetailsSchema(context);

        var example = CreateProblemExample(
            "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            "One or more validation errors occurred.",
            400,
            null,
            new OpenApiObject
            {
                ["errors"] = new OpenApiObject
                {
                    ["SomeData"] = new OpenApiArray
                    {
                        new OpenApiString("The SomeField field is required.")
                    }
                },
                ["traceId"] = new OpenApiString("00-050547f32ae98886f7da6e3851222456-83835d3ebcadb9a8-00")
            });

        badRequest.Content = new Dictionary<string, OpenApiMediaType>
        {
            [MediaTypeNames.Application.ProblemJson] = new() { Schema = schema, Example = example }
        };
    }
}