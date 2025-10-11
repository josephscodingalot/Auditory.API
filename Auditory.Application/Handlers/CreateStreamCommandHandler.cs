using Auditory.Application.Commands;
using Auditory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Stream = Auditory.Domain.Entities.Stream;

namespace Auditory.Application.Handlers;

public class CreateStreamCommandHandler(IStreamRepository streamRepository, ILogger<CreateStreamCommandHandler> logger)
    : IRequestHandler<CreateStreamCommand, Stream>
{
    private readonly IStreamRepository _streamRepository = streamRepository ?? throw new ArgumentNullException(nameof(streamRepository));
    private readonly ILogger<CreateStreamCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Stream> Handle(CreateStreamCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new stream");
        try
        {
            var newStream = new Stream
            {
                UserName = request.userName,
                Platform = request.platform,
                MsPlayed = request.msPlayed,
                ConnCountry = request.connCountry,
                TrackName = request.trackName,
                ArtistName = request.artistName,
                AlbumName = request.albumName,
                SpotifyTrackUri = request.spotifyTrackUri,
                Timestamp = request.timestamp
            };

            await _streamRepository.AddStreamAsync(newStream);
            
            _logger.LogInformation("Stream created with ID: {StreamId}", newStream.Id);
            
            return newStream;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while creating a new stream: {ErrorMessage}", ex.Message);
            throw; // Re-throw the exception after logging it
        }
    }
}