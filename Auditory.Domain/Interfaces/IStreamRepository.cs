namespace Auditory.Domain.Interfaces;

public interface IStreamRepository
{
    Task AddStreamAsync(Stream stream);
    Task<Stream?> GetStreamByIdAsync(Guid id);
    Task<IEnumerable<Stream>> GetAllStreamsAsync();
    Task UpdateStreamAsync(Stream stream);
    Task DeleteStreamAsync(Guid id);
    Task<IEnumerable<Stream>> GetStreamsByUserNameAsync(string userName);
    Task<IEnumerable<Stream>> GetStreamsByArtistNameAsync(string artistName);
    Task<IEnumerable<Stream>> GetStreamsByAlbumNameAsync(string albumName);
    Task<IEnumerable<Stream>> GetStreamsByTrackNameAsync(string trackName);
    Task<IEnumerable<Stream>> GetStreamsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<int> GetTotalStreamsCountAsync();
    Task<int> GetTotalMsPlayedAsync();
    Task<Dictionary<string, int>> GetStreamsCountByPlatformAsync();
    Task<Dictionary<string, int>> GetStreamsCountByCountryAsync();
    Task<Dictionary<string, int>> GetTopArtistsAsync(int topN);
    Task<Dictionary<string, int>> GetTopAlbumsAsync(int topN);
    Task<Dictionary<string, int>> GetTopTracksAsync(int topN);
    Task<double> GetAverageMsPlayedAsync();
    Task<int> GetTotalSkippedStreamsAsync();
    Task<int> GetTotalOfflineStreamsAsync();
    Task<int> GetTotalIncognitoStreamsAsync();
    
}