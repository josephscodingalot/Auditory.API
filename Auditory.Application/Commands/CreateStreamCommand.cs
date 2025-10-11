using MediatR;

namespace Auditory.Application.Commands;

public record CreateStreamCommand(string userName,
    string platform,
    int msPlayed,
    string connCountry,
    string trackName,
    string artistName,
    string albumName,
    string spotifyTrackUri,
    DateTime timestamp) : IRequest<Domain.Entities.Stream>;