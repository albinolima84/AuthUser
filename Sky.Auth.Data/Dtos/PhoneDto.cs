using MongoDB.Bson.Serialization.Attributes;

namespace Sky.Auth.Data.Dtos
{
    public class PhoneDto
    {
        [BsonElement("ddd")]
        public string DDD { get; set; }

        [BsonElement("number")]
        public string Number { get; set; }
    }
}