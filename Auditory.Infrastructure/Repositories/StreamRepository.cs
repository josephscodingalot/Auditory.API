using Auditory.Domain.Interfaces;

namespace Auditory.Infrastructure.Repositories;

public class StreamRepository : IStreamRepository
{
    private readonly MongoDbContext _context;

    public StreamRepository(MongoDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public Task AddStreamAsync(Stream stream)
    {
        throw new NotImplementedException();
    }

    public Task<Stream?> GetStreamByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Stream>> GetAllStreamsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateStreamAsync(Stream stream)
    {
        throw new NotImplementedException();
    }

    public Task DeleteStreamAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Stream>> GetStreamsByUserNameAsync(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Stream>> GetStreamsByArtistNameAsync(string artistName)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Stream>> GetStreamsByAlbumNameAsync(string albumName)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Stream>> GetStreamsByTrackNameAsync(string trackName)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Stream>> GetStreamsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalStreamsCountAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalMsPlayedAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, int>> GetStreamsCountByPlatformAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, int>> GetStreamsCountByCountryAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, int>> GetTopArtistsAsync(int topN)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, int>> GetTopAlbumsAsync(int topN)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, int>> GetTopTracksAsync(int topN)
    {
        throw new NotImplementedException();
    }

    public Task<double> GetAverageMsPlayedAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalSkippedStreamsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalOfflineStreamsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalIncognitoStreamsAsync()
    {
        throw new NotImplementedException();
    }
}