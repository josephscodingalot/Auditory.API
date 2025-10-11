using Auditory.Application.Commands;
using Auditory.Application.Handlers;
using Auditory.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Stream = Auditory.Domain.Entities.Stream;

namespace Auditory.API.Tests.Handlers;

public class CreateStreamHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateStream_WhenDataIsValid()
    {
        // Arrange
        var mockRepo = new Mock<IStreamRepository>();
        var mockLogger = new Mock<ILogger<CreateStreamCommandHandler>>();
        var command = new CreateStreamCommand("joseph", "Spotify", 200000, "US", "Song1", "Artist1", "Album1", "spotify:track:123", DateTime.UtcNow);
        var handler = new CreateStreamCommandHandler(mockRepo.Object, mockLogger.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.UserName.Should().Be("joseph");
        mockRepo.Verify(r => r.AddStreamAsync(It.IsAny<Stream>()), Times.Once);
    }   
    
    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
        // Arrange
        var mockRepo = new Mock<IStreamRepository>();
        var mockLogger = new Mock<ILogger<CreateStreamCommandHandler>>();
        var command = new CreateStreamCommand("joseph", "Spotify", 200000, "US", "Song1", "Artist1", "Album1", "spotify:track:123", DateTime.UtcNow);
        mockRepo.Setup(r => r.AddStreamAsync(It.IsAny<Stream>())).ThrowsAsync(new Exception("Database error"));
        var handler = new CreateStreamCommandHandler(mockRepo.Object, mockLogger.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        mockRepo.Verify(r => r.AddStreamAsync(It.IsAny<Stream>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldLogInformation_WhenStreamIsCreated()
    {
        // Arrange
        var mockRepo = new Mock<IStreamRepository>();
        var mockLogger = new Mock<ILogger<CreateStreamCommandHandler>>();
        var command = new CreateStreamCommand("joseph", "Spotify", 200000, "US", "Song1", "Artist1", "Album1", "spotify:track:123", DateTime.UtcNow);
        var handler = new CreateStreamCommandHandler(mockRepo.Object, mockLogger.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Creating a new stream")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Stream created with ID")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);    
        
    }
}