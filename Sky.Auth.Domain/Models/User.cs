using System.Collections.Generic;

namespace Sky.Auth.Domain.Models
{
    public class User
    {
        public string ObjectId { get; }

        public string Name { get; }

        public string Email { get; }

        public string Password { get; }

        public IReadOnlyCollection<Phone> Phones { get; }

        public User(string objectId, string name, string email, string password, IReadOnlyCollection<Phone> phones)
        {
            ObjectId = objectId;
            Name = name;
            Email = email;
            Password = password;
            Phones = phones;
        }
    }
}
