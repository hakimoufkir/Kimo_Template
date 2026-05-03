using System.ComponentModel.DataAnnotations;
using Kimo.ZLApp.Application.WeatherForecasts.Validators;

namespace Kimo.ZLApp.Application.UnitTests.WeatherForecasts.Validators;

public class IsEvenAttributeTests
{
    private static IsEvenAttribute CreateAttribute()
    {
        return new IsEvenAttribute();
    }

    private static ValidationContext CreateValidationContext(string memberName = "TestProperty")
    {
        return new ValidationContext(new object()) { MemberName = memberName };
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(-2)]
    [InlineData(100)]
    [InlineData(int.MaxValue - 1)]
    public void IsValid_WhenValueIsEven_ReturnsSuccess(int evenNumber)
    {
        // Arrange
        var attribute = CreateAttribute();
        var context = CreateValidationContext();

        // Act
        var result = attribute.GetValidationResult(evenNumber, context);

        // Assert
        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(-1)]
    [InlineData(99)]
    [InlineData(int.MaxValue)]
    public void IsValid_WhenValueIsOdd_ReturnsValidationError(int oddNumber)
    {
        // Arrange
        var attribute = CreateAttribute();
        var context = CreateValidationContext("NumberField");

        // Act
        var result = attribute.GetValidationResult(oddNumber, context);

        // Assert
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("The field NumberField must be even.");
        result.MemberNames.ShouldContain("NumberField");
    }

    [Theory]
    [InlineData("not a number")]
    [InlineData(3.14)]
    [InlineData(true)]
    [InlineData(null)]
    public void IsValid_WhenValueIsNotInteger_ReturnsValidationError(object? invalidValue)
    {
        // Arrange
        var attribute = CreateAttribute();
        var context = CreateValidationContext("NumberField");

        // Act
        var result = attribute.GetValidationResult(invalidValue, context);

        // Assert
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("The field NumberField must be even.");
        result.MemberNames.ShouldContain("NumberField");
    }

    [Fact]
    public void IsValid_WhenMemberNameIsNull_UsesEmptyStringInErrorMessage()
    {
        // Arrange
        var attribute = CreateAttribute();
        var context = new ValidationContext(new object()) { MemberName = null };

        // Act
        var result = attribute.GetValidationResult(1, context);

        // Assert
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("The field  must be even.");
    }

    [Fact]
    public void Constructor_SetsCorrectErrorMessage()
    {
        // Arrange
        var attribute = CreateAttribute();
        var context = CreateValidationContext("TestField");

        // Act
        var result = attribute.GetValidationResult(1, context);

        // Assert
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("The field TestField must be even.");
    }
}
