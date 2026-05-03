using System.Linq.Expressions;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Sorting;

public sealed record SortConfig<T> where T : class
{
    public required string FieldName { get; init; }

    public required Expression<Func<T, object>> PropertyExpression { get; init; }

    public Expression<Func<T, object>>? ThenByExpression { get; init; }
}
