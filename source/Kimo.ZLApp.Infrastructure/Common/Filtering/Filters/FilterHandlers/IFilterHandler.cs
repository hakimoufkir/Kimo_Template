using System.Linq.Expressions;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;

public interface IFilterHandler
{
    Expression? BuildPredicate(Expression property, List<string> values);
}