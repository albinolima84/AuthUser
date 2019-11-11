using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Sky.Auth.Data.Dtos
{
    [BsonIgnoreExtraElements]
    public class UserDto
    {
        [BsonId]
        public ObjectId ObjectId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("phones")]
        public IReadOnlyCollection<PhoneDto> PhoneNumbers { get; set; }

        [BsonElement("created")]
        public DateTime Created { get; set; }

        [BsonElement("updated")]
        public DateTime Updated { get; set; }

        [BsonElement("lastLogin")]
        public DateTime LastLogin { get; set; }

        [BsonElement("token")]
        public string Token { get; set; }
    }
}
