using Auditory.Domain.Interfaces;
using Auditory.Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;
using Stream = Auditory.Domain.Entities.Stream;

namespace Auditory.Infrastructure.Repositories;

public class StreamRepository(MongoDbContext context) : IStreamRepository
{
    private readonly MongoDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddStreamAsync(Stream stream)
    {
        await _context.Streams.InsertOneAsync(stream);
    }
    
    public async Task AddStreamsRangeAsync(IEnumerable<Stream> streams)
    {
        await _context.Streams.InsertManyAsync(streams);
    }

    public async Task<Stream?> GetStreamByIdAsync(Guid id)
    {
        return await _context.Streams.Find(s => s.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Stream>> GetAllStreamsAsync()
    {
        return await _context.Streams.Find(s => true).ToListAsync();
    }

    public async Task UpdateStreamAsync(Stream stream)
    {
        await _context.Streams.ReplaceOneAsync(s => s.Id == stream.Id, stream);
    }

    public async Task DeleteStreamAsync(Guid id)
    {
        await _context.Streams.DeleteOneAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Stream>> GetStreamsByUserNameAsync(string userName)
    {
        return await _context.Streams.Find(s => s.UserName == userName).ToListAsync(); 
    }

    public async Task<IEnumerable<Stream>> GetStreamsByArtistNameAsync(string artistName)
    {
        return await _context.Streams.Find(s => s.ArtistName == artistName).ToListAsync();
    }

    public async Task<IEnumerable<Stream>> GetStreamsByAlbumNameAsync(string albumName)
    {
        return await _context.Streams.Find(s => s.AlbumName == albumName).ToListAsync();
    }

    public async Task<IEnumerable<Stream>> GetStreamsByTrackNameAsync(string trackName)
    {
        return await _context.Streams.Find(s => s.TrackName == trackName).ToListAsync();
    }

    public async Task<IEnumerable<Stream>> GetStreamsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Streams.Find(s => s.Timestamp >= startDate && s.Timestamp <= endDate).ToListAsync();
    }

    public async Task<long> GetTotalStreamsCountAsync()
    {
        return await _context.Streams.Find(s => true).CountDocumentsAsync();
    }

    public async Task<int> GetTotalMsPlayedAsync()
    {
        return await _context.Streams.Aggregate()
            .Group(new BsonDocument 
            { 
                { "_id", BsonNull.Value }, 
                { "TotalMsPlayed", new BsonDocument("$sum", "$MsPlayed") } 
            })
            .FirstOrDefaultAsync()
            .ContinueWith(t => t.Result?["TotalMsPlayed"].AsInt32 ?? 0);
    }

    public async Task<Dictionary<string, int>> GetStreamsCountByPlatformAsync()
    {
        return await _context.Streams.Aggregate()
            .Group(s => s.Platform, g => new { Platform = g.Key, Count = g.Count() })
            .ToListAsync()
            .ContinueWith(t => t.Result.ToDictionary(x => x.Platform, x => x.Count));
    }

    public async Task<Dictionary<string, int>> GetStreamsCountByCountryAsync()
    {
        return await _context.Streams.Aggregate()
            .Group(s => s.ConnCountry, g => new { Country = g.Key, Count = g.Count() })
            .ToListAsync()
            .ContinueWith(t => t.Result.ToDictionary(x => x.Country, x => x.Count));
    }

    public async Task<Dictionary<string, int>> GetTopArtistsAsync(int topN)
    {
        return await _context.Streams.Aggregate()
            .Group(s => s.ArtistName, g => new { Artist = g.Key, Count = g.Count() })
            .SortByDescending(g => g.Count)
            .Limit(topN)
            .ToListAsync()
            .ContinueWith(t => t.Result.ToDictionary(x => x.Artist, x => x.Count));
    }

    public async Task<Dictionary<string, int>> GetTopAlbumsAsync(int topN)
    {
        return await _context.Streams.Aggregate()
            .Group(s => s.AlbumName, g => new { Album = g.Key, Count = g.Count() })
            .SortByDescending(g => g.Count)
            .Limit(topN)
            .ToListAsync()
            .ContinueWith(t => t.Result.ToDictionary(x => x.Album, x => x.Count));
    }

    public async Task<Dictionary<string, int>> GetTopTracksAsync(int topN)
    {
        return await _context.Streams.Aggregate()
            .Group(s => s.TrackName, g => new { Track = g.Key, Count = g.Count() })
            .SortByDescending(g => g.Count)
            .Limit(topN)
            .ToListAsync()
            .ContinueWith(t => t.Result.ToDictionary(x => x.Track, x => x.Count));
    }
    
}