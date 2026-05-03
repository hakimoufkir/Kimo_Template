using System.ComponentModel.DataAnnotations;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;

namespace Kimo.ZLApp.Application.UnitTests.Common.RequestPipelines;

public class TestRequest : IRequest<Result<string>>
{
    [Required] [MinLength(10)] public required string Name { get; set; }
}
