using Auditory.Application.Queries;
using Auditory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Stream = Auditory.Domain.Entities.Stream;

namespace Auditory.Application.Handlers;

public class GetStreamByIdHandler(IStreamRepository streamRepository, ILogger<GetStreamByIdHandler> logger)
    : IRequestHandler<GetStreamByIdQuery, Stream>
{
    private readonly IStreamRepository _streamRepository = streamRepository ?? throw new ArgumentNullException(nameof(streamRepository));
    private readonly ILogger<GetStreamByIdHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Stream> Handle(GetStreamByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching stream with ID: {StreamId}", request.StreamId);
        try
        {
            if (request.StreamId == Guid.Empty)
            {
                _logger.LogError("Stream with ID: {StreamId} does not exist", request?.StreamId);
                throw new ArgumentException("Stream ID cannot be null", nameof(request.StreamId));
            }

            var stream = await _streamRepository.GetStreamByIdAsync(request.StreamId);
            if (stream is null)
            {
                _logger.LogWarning("No stream found with ID: {StreamId}", request.StreamId);
                return null;
            }

            _logger.LogInformation("Stream found with ID: {StreamId}", request.StreamId);
            return stream;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while fetching stream with ID {StreamId}: {ErrorMessage}", request.StreamId, ex.Message);
            throw; // Re-throw the exception after logging it
        }
    }
}
