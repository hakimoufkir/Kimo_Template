using Hangfire;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.Storage;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.Error;
using Kimo.ZLApp.Infrastructure.Jobs.Common;
using NSubstitute;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.Jobs.UnitTests.Common;

public class FailOnErrorAttributeTests
{
    private static PerformingContext CreatePerformingContext()
    {
        var storage = Substitute.For<JobStorage>();
        var storageConnection = Substitute.For<IStorageConnection>();

        var job = new Job(typeof(TestJob).GetMethod(nameof(TestJob.Execute)));
        var backgroundJob = new BackgroundJob("Id", job, new DateTime(2026, 1, 1));
        var cancellationToken = Substitute.For<IJobCancellationToken>();

        var performContext = new PerformContext(storage, storageConnection, backgroundJob, cancellationToken);

        return new PerformingContext(performContext);
    }

    private static PerformedContext CreatePerformedContext(object result)
    {
        var storage = Substitute.For<JobStorage>();
        var storageConnection = Substitute.For<IStorageConnection>();
        var job = new Job(typeof(TestJob).GetMethod(nameof(TestJob.Execute)));
        var backgroundJob = new BackgroundJob("Id", job, new DateTime(2026, 1, 1));
        var cancellationToken = Substitute.For<IJobCancellationToken>();

        var performContext = new PerformContext(storage, storageConnection, backgroundJob, cancellationToken);

        return new PerformedContext(performContext, result, false, null);
    }

    private static Result CreateFailedResult(ErrorType type, string message = "failure",
        Exception? metadataException = null)
    {
        return new Error(type, message)
        {
            Metadata = metadataException is null
                ? null
                : new Dictionary<string, object> { ["exception"] = metadataException }
        };
    }

    [Fact]
    public void OnPerformed_ShouldNotThrow_WhenResultIsNotAResult()
    {
        // Arrange
        var attr = new FailOnErrorAttribute();
        var context = CreatePerformedContext("not a result instance");

        // Act & Assert
        Should.NotThrow(() => attr.OnPerformed(context));
    }

    [Fact]
    public void OnPerformed_ShouldNotThrow_WhenResultIsSuccess()
    {
        // Arrange
        var attr = new FailOnErrorAttribute();
        var context = CreatePerformedContext(Result.Success());

        // Act & Assert
        Should.NotThrow(() => attr.OnPerformed(context));
    }

    [Fact]
    public void OnPerformed_ShouldThrow_WhenFailureAndNoFiltersProvided()
    {
        // Arrange
        var attr = new FailOnErrorAttribute();
        var context = CreatePerformedContext(CreateFailedResult(ErrorType.Unexpected));

        // Act & Assert
        Should.Throw<ApplicationException>(() => attr.OnPerformed(context));
    }

    [Fact]
    public void OnPerformed_ShouldThrow_WhenFailureMatchingErrorFilter()
    {
        // Arrange
        var attr = new FailOnErrorAttribute(ErrorType.Validation);
        var context = CreatePerformedContext(CreateFailedResult(ErrorType.Validation));

        // Act & Assert
        var ex = Should.Throw<ApplicationException>(() => attr.OnPerformed(context));
        ex.Message.ShouldContain("Validation");
    }

    [Fact]
    public void OnPerformed_ShouldNotThrow_WhenFailureNotMatchingErrorFilter()
    {
        // Arrange
        var attr = new FailOnErrorAttribute(ErrorType.Validation);
        var context = CreatePerformedContext(CreateFailedResult(ErrorType.Unexpected));

        // Act & Assert
        Should.NotThrow(() => attr.OnPerformed(context));
    }

    [Fact]
    public void OnPerformed_ShouldUseMetadataException_WhenAvailable()
    {
        // Arrange
        var exception = new ApplicationException("Injected Test Exception");
        var attr = new FailOnErrorAttribute();
        var context = CreatePerformedContext(CreateFailedResult(ErrorType.Unexpected, metadataException: exception));

        // Act
        var ex = Should.Throw<ApplicationException>(() => attr.OnPerformed(context));

        // Assert
        ex.Message.ShouldContain("Injected Test Exception");
    }

    [Fact]
    public void OnPerforming_ShouldNotThrow_WithDefaultConstructor()
    {
        // Arrange
        var attr = new FailOnErrorAttribute();
        var context = CreatePerformingContext();

        // Act & Assert
        Should.NotThrow(() => attr.OnPerforming(context));
    }

    [Fact]
    public void OnPerforming_ShouldNotThrow_WithErrorFilters()
    {
        // Arrange
        var attr = new FailOnErrorAttribute(ErrorType.Validation, ErrorType.Unexpected);
        var context = CreatePerformingContext();

        // Act & Assert
        Should.NotThrow(() => attr.OnPerforming(context));
    }

    [Fact]
    public void OnPerforming_ShouldReceiveContext()
    {
        // Arrange
        var attr = new FailOnErrorAttribute();
        var context = CreatePerformingContext();

        // Act
        Should.NotThrow(() => attr.OnPerforming(context));

        // Assert
        context.BackgroundJob.ShouldNotBeNull();
        context.Connection.ShouldNotBeNull();
        context.Storage.ShouldNotBeNull();
    }
}
