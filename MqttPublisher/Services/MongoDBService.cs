using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MqttPublisher.Configurations;

public interface IMongoDBService
{
    void Insert<T>(T document);
    void Update<T>(FilterDefinition<T> filter, UpdateDefinition<T> update);
    void Delete<T>(FilterDefinition<T> filter);
    List<T> GetAll<T>(FilterDefinition<T> filter = null);
}

public class MongoDBService : IMongoDBService
{
    private readonly IMongoDatabase _database;

    public MongoDBService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public void Insert<T>(T document)
    {
        var collection = _database.GetCollection<T>(typeof(T).Name);
        collection.InsertOne(document);
    }

    public void Update<T>(FilterDefinition<T> filter, UpdateDefinition<T> update)
    {
        var collection = _database.GetCollection<T>(typeof(T).Name);
        filter = filter ?? Builders<T>.Filter.Empty;
        collection.UpdateOne(filter, update);
    }

    public void Delete<T>(FilterDefinition<T> filter)
    {
        var collection = _database.GetCollection<T>(typeof(T).Name);
        filter = filter ?? Builders<T>.Filter.Empty;
        collection.DeleteOne(filter);
    }

    public List<T> GetAll<T>(FilterDefinition<T> filter = null)
    {
        var collection = _database.GetCollection<T>(typeof(T).Name);
        filter = filter ?? Builders<T>.Filter.Empty;
        return collection.Find(filter).ToList();
    }
}