namespace Kimo.ZLApp.Application.Common.Results.ResultModels;

public record DeletedResult(int? Requested, int Successful, int NotFound) : Result;