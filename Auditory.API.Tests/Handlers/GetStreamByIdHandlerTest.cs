using Auditory.Application.Handlers;
using Auditory.Application.Queries;
using Auditory.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Auditory.API.Tests.Handlers;
using Stream = Auditory.Domain.Entities.Stream;

public class GetStreamByIdHandlerTest
{
    [Fact]
    public async Task Handle_ShouldReturnStream_WhenStreamExists()
    {
        //Arrange
        var mockRepo = new Mock<IStreamRepository>();
        var mockLogger = new Mock<ILogger<GetStreamByIdHandler>>();
        var streamId = Guid.NewGuid();
        var expectedStream = new Stream("joseph", "Spotify", 200000, "US", "Song1", "Artist1", "Album1", "spotify:track:123", DateTime.UtcNow)
        {
            Id = streamId
        };
        
        mockRepo.Setup(r => r.GetStreamByIdAsync(streamId))
            .ReturnsAsync(expectedStream);
        var handler = new GetStreamByIdHandler(mockRepo.Object, mockLogger.Object);
        
        //Act
        var result = await handler.Handle(new GetStreamByIdQuery(streamId), CancellationToken.None);
        //Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(streamId);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenStreamDoesNotExist()
    {
        //Arrange
        var mockRepo = new Mock<IStreamRepository>();
        var mockLogger = new Mock<ILogger<GetStreamByIdHandler>>();
        var streamId = Guid.NewGuid();
        
        mockRepo.Setup(r => r.GetStreamByIdAsync(streamId))
            .ReturnsAsync((Stream?)null);
        var handler = new GetStreamByIdHandler(mockRepo.Object, mockLogger.Object);
        
        //Act
        var result = await handler.Handle(new GetStreamByIdQuery(streamId), CancellationToken.None);
        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenStreamIdIsEmpty()
    {
        //Arrange
        var mockRepo = new Mock<IStreamRepository>();
        var mockLogger = new Mock<ILogger<GetStreamByIdHandler>>();
        var handler = new GetStreamByIdHandler(mockRepo.Object, mockLogger.Object);
        //Act
        Func<Task> act = async () => await handler.Handle(new GetStreamByIdQuery(Guid.Empty), CancellationToken.None);
        //Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Stream ID cannot be null*");  
    }
}