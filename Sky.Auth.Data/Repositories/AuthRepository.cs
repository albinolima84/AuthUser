using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sky.Auth.CrossCutting.Options;
using Sky.Auth.Data.Connection;
using Sky.Auth.Data.Dtos;
using Sky.Auth.Data.Extensions;
using Sky.Auth.Domain.Interfaces;
using Sky.Auth.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sky.Auth.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private IMongoCollection<UserDto> _mongoCollection;
        private readonly IConnect _connect;
        private readonly string _database;
        private readonly string _collection;

        public AuthRepository(IConnect connect, IOptions<MongoOptions> config)
        {
            _database = config?.Value?.Database;
            _collection = config?.Value?.Collection;
            SetConnectAndCollection(connect);
            _connect = connect;
        }

        internal void SetConnectAndCollection(IConnect connect) => _mongoCollection = connect.Collection<UserDto>(_collection, _database);

        public async Task<User> CreateUser(User user)
        {
            var userDto = user.ToDto();

            await _mongoCollection.InsertOneAsync(userDto);

            return userDto.ToDomain();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            UserDto user = null;

            var filter = Builders<UserDto>.Filter.Eq("email", email);

            using (IAsyncCursor<UserDto> cursor = await _mongoCollection.FindAsync<UserDto>(filter))
            {
                await cursor.MoveNextAsync();
                if (cursor.Current.Any())
                {
                    user = cursor.Current.FirstOrDefault();
                }
            }

            return user?.ToDomain();
        }

        public async Task<User> Authenticate(string email, string password)
        {
            UserDto user = null;

            var emailFilter = new FilterDefinitionBuilder<UserDto>().Eq("email", email);
            var passwordFilter = new FilterDefinitionBuilder<UserDto>().Eq("password", password);
            var filter = Builders<UserDto>.Filter.And(emailFilter, passwordFilter);

            using (IAsyncCursor<UserDto> cursor = await _mongoCollection.FindAsync<UserDto>(filter))
            {
                await cursor.MoveNextAsync();
                if (cursor.Current.Any())
                {
                    user = cursor.Current.FirstOrDefault();
                }
            }

            return user?.ToDomain();
        }

        public async Task<bool> UpdateLastLogin(User user)
        {
            var userDto = user.ToDto();
            userDto.LastLogin = DateTime.UtcNow;

            var filter = new FilterDefinitionBuilder<UserDto>().Eq("_id", userDto.ObjectId);

            var result = await _mongoCollection.ReplaceOneAsync(filter, userDto);

            return result.MatchedCount > 0;
        }
    }
}
