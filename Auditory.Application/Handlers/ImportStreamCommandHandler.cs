using Auditory.Application.Commands;
using Auditory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Stream = Auditory.Domain.Entities.Stream;

namespace Auditory.Application.Handlers;

public class ImportStreamCommandHandler(IStreamRepository streamRepository, ILogger<ImportStreamCommandHandler> logger)
    : IRequestHandler<ImportStreamCommand, List<Stream>>
{
    private readonly IStreamRepository _streamRepository = streamRepository ?? throw new ArgumentNullException(nameof(streamRepository));
    private readonly ILogger<ImportStreamCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<List<Stream>> Handle(ImportStreamCommand request, CancellationToken cancellationToken)
    {
        if (request.spotifyData.Count == 0)
        {
            _logger.LogWarning("No data to import.");
            return [];
        }

        var importedStreams = new List<Stream>();

        foreach (var stream in request.spotifyData)
        {
            try
            {
                var existingStream = await _streamRepository.GetSteamByTimestampAndUserAsync(stream.Timestamp, stream.UserName);
                // if (existingStream != null)
                // {
                //     _logger.LogInformation($"Stream already exists for user {stream.UserName} at {stream.Timestamp}. Skipping import.");
                //     continue;
                // }

                await _streamRepository.AddStreamAsync(stream);
                importedStreams.Add(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error importing stream for user {stream.UserName} at {stream.Timestamp}");
            }
        }

        return importedStreams;
    }
    
}