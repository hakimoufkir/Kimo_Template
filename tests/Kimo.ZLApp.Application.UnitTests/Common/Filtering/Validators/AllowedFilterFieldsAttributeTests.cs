using System.ComponentModel.DataAnnotations;
using Kimo.ZLApp.Application.Common.Filtering.Requests;
using Kimo.ZLApp.Application.Common.Filtering.Validators;

namespace Kimo.ZLApp.Application.UnitTests.Common.Filtering.Validators;

public class AllowedFilterFieldsAttributeTests
{
    private static AllowedFilterFieldsAttribute CreateAttribute(params string[] allowedFields)
    {
        return new AllowedFilterFieldsAttribute(allowedFields);
    }

    private static ValidationContext CreateValidationContext(string memberName = "Filters")
    {
        return new ValidationContext(new object()) { MemberName = memberName };
    }

    [Fact]
    public void IsValid_WhenValueIsNull_ReturnsSuccess()
    {
        // Arrange
        var attribute = CreateAttribute("Name", "Age");
        var context = CreateValidationContext();

        // Act
        var result = attribute.GetValidationResult(null, context);

        // Assert
        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WhenValueIsNotFilterOptions_ReturnsSuccess()
    {
        // Arrange
        var attribute = CreateAttribute("Name", "Age");
        var context = CreateValidationContext();

        // Act
        var result = attribute.GetValidationResult("not a FilterOptions", context);

        // Assert
        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WithEmptyFilters_ReturnsSuccess()
    {
        // Arrange
        var attribute = CreateAttribute("Name", "Age");
        var context = CreateValidationContext();
        var filterOptions = new FilterOptions { Filters = [] };

        // Act
        var result = attribute.GetValidationResult(filterOptions, context);

        // Assert
        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WithAllAllowedFields_ReturnsSuccess()
    {
        // Arrange
        var attribute = CreateAttribute("Name", "Age", "Email");
        var context = CreateValidationContext();
        var filterOptions = new FilterOptions
        {
            Filters =
            [
                new FilterOption { Field = "Name", Value = "John" },
                new FilterOption { Field = "Age", Value = "25" },
                new FilterOption { Field = "Email", Value = "@test.com" }
            ]
        };

        // Act
        var result = attribute.GetValidationResult(filterOptions, context);

        // Assert
        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WithDisallowedField_ReturnsValidationError()
    {
        // Arrange
        var attribute = CreateAttribute("Name", "Age");
        var context = CreateValidationContext();
        var filterOptions = new FilterOptions
        {
            Filters =
            [
                new FilterOption { Field = "Email", Value = "test@test.com" }
            ]
        };

        // Act
        var result = attribute.GetValidationResult(filterOptions, context);

        // Assert
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("'Email' is not an allowed filter field. Allowed: Name, Age");
        result.MemberNames.ShouldContain("Filters");
    }

    [Fact]
    public void IsValid_WithMixedAllowedAndDisallowed_ReturnsErrorForFirstDisallowed()
    {
        // Arrange
        var attribute = CreateAttribute("Name", "Age");
        var context = CreateValidationContext();
        var filterOptions = new FilterOptions
        {
            Filters =
            [
                new FilterOption { Field = "Name", Value = "John" },
                new FilterOption { Field = "Email", Value = "test@test.com" },
                new FilterOption { Field = "Phone", Value = "123" }
            ]
        };

        // Act
        var result = attribute.GetValidationResult(filterOptions, context);

        // Assert
        result.ShouldNotBeNull();
        result.ErrorMessage?.ShouldContain("'Email' is not an allowed filter field");
    }

    [Theory]
    [InlineData("name")]
    [InlineData("NAME")]
    [InlineData("NaMe")]
    public void IsValid_IsCaseInsensitive_ReturnsSuccess(string fieldName)
    {
        // Arrange
        var attribute = CreateAttribute("Name", "Age");
        var context = CreateValidationContext();
        var filterOptions = new FilterOptions
        {
            Filters =
            [
                new FilterOption { Field = fieldName, Value = "John" }
            ]
        };

        // Act
        var result = attribute.GetValidationResult(filterOptions, context);

        // Assert
        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WithWhitespaceInField_TrimsAndValidates()
    {
        // Arrange
        var attribute = CreateAttribute("Name", "Age");
        var context = CreateValidationContext();
        var filterOptions = new FilterOptions
        {
            Filters =
            [
                new FilterOption { Field = "  Name  ", Value = "John" }
            ]
        };

        // Act
        var result = attribute.GetValidationResult(filterOptions, context);

        // Assert
        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WithWhitespaceInAllowedFields_TrimsCorrectly()
    {
        // Arrange
        var attribute = CreateAttribute("  Name  ", "  Age  ");
        var context = CreateValidationContext();
        var filterOptions = new FilterOptions
        {
            Filters =
            [
                new FilterOption { Field = "Name", Value = "John" }
            ]
        };

        // Act
        var result = attribute.GetValidationResult(filterOptions, context);

        // Assert
        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void Constructor_WithNoAllowedFields_CreatesEmptyHashSet()
    {
        // Arrange & Act
        var attribute = CreateAttribute();
        var context = CreateValidationContext();
        var filterOptions = new FilterOptions
        {
            Filters =
            [
                new FilterOption { Field = "Name", Value = "John" }
            ]
        };

        // Act
        var result = attribute.GetValidationResult(filterOptions, context);

        // Assert
        result.ShouldNotBeNull();
        result.ErrorMessage?.ShouldContain("'Name' is not an allowed filter field");
    }
}
