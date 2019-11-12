using System;
using System.Collections.Generic;

namespace Sky.Auth.Domain.Models
{
    public class User
    {
        public string Id { get; }

        public string Name { get; }

        public string Email { get; }

        public string Password { get; }

        public IReadOnlyCollection<Phone> Phones { get; }

        public DateTime Created { get; }

        public DateTime Updated { get; }

        public DateTime LastLogin { get; }

        public string Token { get; }

        public User(string objectId, string name, string email, string password, IReadOnlyCollection<Phone> phones)
        {
            Id = objectId;
            Name = name;
            Email = email;
            Password = password;
            Phones = phones;
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
            LastLogin = DateTime.UtcNow;
        }

        public User(string objectId, string name, string email, string password, IReadOnlyCollection<Phone> phones, DateTime created, DateTime updated, DateTime lastLogin, string token)
        {
            Id = objectId;
            Name = name;
            Email = email;
            Password = password;
            Phones = phones;
            Created = created;
            Updated = updated;
            LastLogin = lastLogin;
            Token = token;
        }
    }
}
