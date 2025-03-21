using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Backend.Data.Data.SQL_Read;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}