using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Filters;

public class FilterHandlerFactoryTests
{
    [Theory]
    [InlineData(FilterType.Equals, typeof(EqualsFilterHandler))]
    [InlineData(FilterType.In, typeof(InFilterHandler))]
    [InlineData(FilterType.Contains, typeof(ContainsFilterHandler))]
    [InlineData(FilterType.DateEquals, typeof(DateEqualsFilterHandler))]
    public void GetHandler_ShouldReturnCorrectHandler(FilterType filterType, Type expectedType)
    {
        // Act
        var handler = FilterHandlerFactory.GetHandler(filterType);

        // Assert
        handler.ShouldNotBeNull();
        handler.ShouldBeOfType(expectedType);
    }

    [Fact]
    public void GetHandler_ShouldThrowForUnsupportedFilterType()
    {
        // Arrange
        var unsupportedType = (FilterType)999;

        // Act & Assert
        Should.Throw<NotSupportedException>(() =>
            FilterHandlerFactory.GetHandler(unsupportedType));
    }
}
