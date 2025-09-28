using Auditory.Application.Queries;
using Auditory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Stream = Auditory.Domain.Entities.Stream;

namespace Auditory.Application.Handlers;

public class GetStreamsByUserHandler : IRequestHandler<GetStreamsByUserQuery, IEnumerable<Stream>>
{
    private readonly ILogger<GetStreamsByUserHandler> _logger;
    private readonly IStreamRepository _streamRepository;

    public GetStreamsByUserHandler(ILogger<GetStreamsByUserHandler> logger, IStreamRepository streamRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _streamRepository = streamRepository ?? throw new ArgumentNullException(nameof(streamRepository));
    }
    
    public async Task<IEnumerable<Stream>> Handle(GetStreamsByUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching streams for user: {UserName}", request.userName);
        try
        {
            if (string.IsNullOrEmpty(request.userName))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(request.userName));
            }
            
            var streams = await _streamRepository.GetStreamsByUserNameAsync(request.userName);
            if(streams.Any())
            {
                _logger.LogInformation("Found {Count} streams for user: {UserName}", streams.Count(), request.userName);
                return streams;
            }
            else
            {
                _logger.LogWarning("No streams found for user: {UserName}", request.userName);
            }

            return [];
        }
        catch (Exception ex)
        {
            // Log the exception (you can use any logging framework you prefer)
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw; // Re-throw the exception after logging it
        }

    }
}