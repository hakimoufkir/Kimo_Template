using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.Error;

namespace Kimo.ZLApp.Application.Common.RequestPipelines.Behaviors;

public static class BehaviorResult
{
    private static readonly Type s_resultGenericType = typeof(Result<>);

    public static bool IsResult<TResponse>()
    {
        var type = typeof(TResponse);

        return typeof(Result).IsAssignableFrom(type) ||
               (type.IsGenericType && type.GetGenericTypeDefinition() == s_resultGenericType);
    }

    public static TResponse Fail<TResponse>(Error error)
    {
        var responseType = typeof(TResponse);

        if (responseType.IsGenericType &&
            responseType.GetGenericTypeDefinition() == s_resultGenericType)
        {
            var innerType = responseType.GetGenericArguments()[0];
            var resultType = typeof(Result<>).MakeGenericType(innerType);
            var instance = Activator.CreateInstance(resultType, error)!;
            return (TResponse)instance;
        }

        if (typeof(Result).IsAssignableFrom(responseType))
        {
            return (TResponse)(object)new Result(error);
        }

        throw new InvalidOperationException(
            $"The response type '{responseType}' does not implement Result or Result<T>.");
    }
}