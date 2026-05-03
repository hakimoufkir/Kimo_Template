using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.WeatherForecasts.Commands;
using Kimo.ZLApp.Infrastructure.Jobs.WeatherForecasts;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.Jobs.UnitTests.WeatherForecasts;

public class ForecastCleanupJobTests
{
    private readonly IRequestExecutor _requestExecutor;
    private readonly ForecastCleanupJob _sut;

    public ForecastCleanupJobTests()
    {
        _requestExecutor = Substitute.For<IRequestExecutor>();
        _sut = new ForecastCleanupJob(_requestExecutor);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallRequestExecutorWithCorrectCommand()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.ExecuteAsync(cancellationToken);

        // Assert
        await _requestExecutor.Received(1).ExecuteAsync(
            Arg.Is<DeleteForecastsBeforeDateCommand>(cmd => cmd.CutoffDays == 30),
            cancellationToken);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRequestExecutorThrowsException_ShouldPropagateException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Test exception");

        _requestExecutor
            .ExecuteAsync(Arg.Any<DeleteForecastsBeforeDateCommand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        // Act & Assert
        var actualException = await Should.ThrowAsync<InvalidOperationException>(() => _sut.ExecuteAsync());
        actualException.ShouldBe(expectedException);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldOnlyCallRequestExecutorOnce()
    {
        // Act
        await _sut.ExecuteAsync();

        // Assert
        await _requestExecutor.Received(1).ExecuteAsync(
            Arg.Any<DeleteForecastsBeforeDateCommand>(),
            Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ExecuteAsync_WithDifferentCancellationTokenStates_ShouldPassCorrectToken(bool isCancelled)
    {
        // Arrange
        var cancellationToken = new CancellationToken(isCancelled);

        // Act
        await _sut.ExecuteAsync(cancellationToken);

        // Assert
        await _requestExecutor.Received(1).ExecuteAsync(
            Arg.Any<DeleteForecastsBeforeDateCommand>(),
            Arg.Is<CancellationToken>(ct => ct.IsCancellationRequested == isCancelled));
    }
}
