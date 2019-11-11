using MongoDB.Driver;

namespace Sky.Auth.Data.Connection
{
    public interface IConnect
    {
        IMongoCollection<T> Collection<T>(string collectionName, string database);
    }
}
