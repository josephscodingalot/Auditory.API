using MongoDB.Driver;

namespace Auditory.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    
    public IMongoCollection<Domain.Entities.Stream> Streams =>
        _database.GetCollection<Domain.Entities.Stream>("Streams");

    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }
    
    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}