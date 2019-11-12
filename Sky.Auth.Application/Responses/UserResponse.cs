using Sky.Auth.Domain.Models;

namespace Sky.Auth.Application.Responses
{
    public class UserResponse
    {
        public User User { get; }

        public UserResponse(User user)
        {
            User = user;
        }
    }
}
