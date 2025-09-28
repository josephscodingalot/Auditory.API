namespace Auditory.Domain.Interfaces;
using Stream = Auditory.Domain.Entities.Stream;

public interface IStreamRepository
{
    Task AddStreamAsync(Stream stream);
    Task AddStreamsRangeAsync(IEnumerable<Stream> streams);
    Task<Stream?> GetStreamByIdAsync(Guid id);
    Task<IEnumerable<Stream>> GetAllStreamsAsync();
    Task UpdateStreamAsync(Stream stream);
    Task DeleteStreamAsync(Guid id);
    Task<IEnumerable<Stream>> GetStreamsByUserNameAsync(string userName);
    Task<IEnumerable<Stream>> GetStreamsByArtistNameAsync(string artistName);
    Task<IEnumerable<Stream>> GetStreamsByAlbumNameAsync(string albumName);
    Task<IEnumerable<Stream>> GetStreamsByTrackNameAsync(string trackName);
    Task<IEnumerable<Stream>> GetStreamsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<long> GetTotalStreamsCountAsync();
    Task<int> GetTotalMsPlayedAsync();
    Task<Dictionary<string, int>> GetStreamsCountByPlatformAsync();
    Task<Dictionary<string, int>> GetStreamsCountByCountryAsync();
    Task<Dictionary<string, int>> GetTopArtistsAsync(int topN);
    Task<Dictionary<string, int>> GetTopAlbumsAsync(int topN);
    Task<Dictionary<string, int>> GetTopTracksAsync(int topN);
}