using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using Sky.Auth.CrossCutting.Options;
using Sky.Auth.Data.Connection;
using Sky.Auth.Data.Dtos;
using Sky.Auth.Data.Extensions;
using Sky.Auth.Domain.Interfaces;
using Sky.Auth.Domain.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sky.Auth.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private IMongoCollection<UserDto> _mongoCollection;
        private readonly IConnect _connect;
        private readonly string _database;
        private readonly string _collection;
        private readonly string _secret;

        public AuthRepository(IConnect connect, IOptions<MongoOptions> config, IOptions<TokenOptions> configToken)
        {
            _database = config?.Value?.Database;
            _collection = config?.Value?.Collection;
            SetConnectAndCollection(connect);
            _connect = connect;
            _secret = configToken?.Value?.Secret;
        }

        internal void SetConnectAndCollection(IConnect connect) => _mongoCollection = connect.Collection<UserDto>(_collection, _database);

        public async Task<User> CreateUser(User user)
        {
            var userDto = user.ToDto();

            await _mongoCollection.InsertOneAsync(userDto);

            GenerateToken(userDto);

            await UpdateUser(userDto);

            return userDto.ToDomain();
        }

        private void GenerateToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.ObjectId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
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

            GenerateToken(userDto);

            return await UpdateUser(userDto);
        }

        public async Task<User> GetUserById(string objectId)
        {
            UserDto user = null;

            var filter = Builders<UserDto>.Filter.Eq("_id", new ObjectId(objectId));

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

        private async Task<bool> UpdateUser(UserDto userDto)
        {
            var filter = new FilterDefinitionBuilder<UserDto>().Eq("_id", userDto.ObjectId);

            var result = await _mongoCollection.ReplaceOneAsync(filter, userDto);

            return result.MatchedCount > 0;
        }
    }
}
