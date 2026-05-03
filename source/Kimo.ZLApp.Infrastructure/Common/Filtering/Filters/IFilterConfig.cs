namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;

public interface IFilterConfig<T> where T : class
{
    string FieldName { get; }

    FilterType Type { get; }

    IQueryable<T> Apply(IQueryable<T> query, List<string> values);
}
