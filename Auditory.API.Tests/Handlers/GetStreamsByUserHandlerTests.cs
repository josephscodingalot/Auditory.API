using Auditory.Application.Handlers;
using Auditory.Application.Queries;
using Auditory.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Stream = Auditory.Domain.Entities.Stream;

namespace Auditory.API.Tests.Handlers;

public class GetStreamsByUserHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnStreams_WhenUserExists()
    {
        // Arrange
        var mockRepo = new Mock<IStreamRepository>();
        var mockLogger = new Mock<ILogger<GetStreamsByUserHandler>>();

        var expectedStreams = new List<Stream>
        {
            new("joseph", "Spotify", 200000, "US", "Song1", "Artist1", "Album1", "spotify:track:123", DateTime.UtcNow)
        };

        mockRepo.Setup(r => r.GetStreamsByUserNameAsync("joseph"))
            .ReturnsAsync(expectedStreams);

        var handler = new GetStreamsByUserHandler(mockLogger.Object, mockRepo.Object);

        // Act
        var result = await handler.Handle(new GetStreamsByUserQuery("joseph"), CancellationToken.None);

        // Assert
        var enumerable = result.ToList();
        
        enumerable.Should().HaveCount(1);
        enumerable.First().UserName.Should().Be("joseph");
    }
    
    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenUserDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IStreamRepository>();
        var mockLogger = new Mock<ILogger<GetStreamsByUserHandler>>();

        mockRepo.Setup(r => r.GetStreamsByUserNameAsync("nonexistentuser"))
            .ReturnsAsync(new List<Stream>());

        var handler = new GetStreamsByUserHandler(mockLogger.Object, mockRepo.Object);

        // Act
        var result = await handler.Handle(new GetStreamsByUserQuery("nonexistentuser"), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}