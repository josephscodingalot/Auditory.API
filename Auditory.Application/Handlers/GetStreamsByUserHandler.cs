using Auditory.Application.Queries;
using Auditory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Stream = Auditory.Domain.Entities.Stream;

namespace Auditory.Application.Handlers;

public class GetStreamsByUserHandler(ILogger<GetStreamsByUserHandler> logger, IStreamRepository streamRepository)
    : IRequestHandler<GetStreamsByUserQuery, IEnumerable<Stream>>
{
    private readonly ILogger<GetStreamsByUserHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IStreamRepository _streamRepository = streamRepository ?? throw new ArgumentNullException(nameof(streamRepository));

    public async Task<IEnumerable<Stream>> Handle(GetStreamsByUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching streams for user: {UserName}", request.userName);
        try
        {
            if (string.IsNullOrEmpty(request.userName))
                throw new ArgumentException("Username cannot be null or empty");
            
            var streams = await _streamRepository.GetStreamsByUserNameAsync(request.userName);
            var enumerable = streams.ToList();
            if(enumerable.Count != 0)
            {
                _logger.LogInformation("Found {Count} streams for user: {UserName}", enumerable.Count, request.userName);
                return enumerable;
            }

            _logger.LogWarning("No streams found for user: {UserName}", request.userName);

            return [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw; 
        }
    }
}