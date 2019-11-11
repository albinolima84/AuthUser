using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sky.Auth.CrossCutting.Options;

namespace Sky.Auth.Data.Connection
{
    public class Connect : IConnect
    {
        protected MongoClient Client { get; }

        protected IMongoDatabase DataBase { get; private set; }

        public IMongoCollection<T> Collection<T>(string collectionName, string database)
        {
            DataBase = Client.GetDatabase(database);
            return DataBase.GetCollection<T>(collectionName);
        }

        public Connect(IOptions<MongoOptions> config)
        {
            Client = new MongoClient(config?.Value?.ConnectionString);
        }
    }
}
