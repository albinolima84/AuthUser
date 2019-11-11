using Sky.Auth.Domain.Models;

namespace Sky.Auth.Command.Responses
{
    public class CreateUserResponse
    {
        public User User { get; }

        public CreateUserResponse(User user)
        {
            User = user;
        }
    }
}
