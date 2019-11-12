using MediatR;
using Sky.Auth.Application.Responses;
using System.Collections.Generic;

namespace Sky.Auth.Application.Commands
{
    public class CreateUserCommand : IRequest<Response<UserResponse>>
    {
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
        public IReadOnlyCollection<PhoneNumberCommand> PhoneNumbers { get; }

        public CreateUserCommand(string name, string email, string password, IReadOnlyCollection<PhoneNumberCommand> phoneNumbers)
        {
            Name = name;
            Email = email;
            Password = password;
            PhoneNumbers = phoneNumbers;
        }
    }
}
