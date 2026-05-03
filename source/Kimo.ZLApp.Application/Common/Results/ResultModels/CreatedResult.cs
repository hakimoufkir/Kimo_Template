namespace Kimo.ZLApp.Application.Common.Results.ResultModels;

public record CreatedResult<T>(string Location, T Value) : Result<T>(Value);