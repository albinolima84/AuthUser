using MediatR;
using Sky.Auth.Application.Responses;

namespace Sky.Auth.Application.Queries
{
    public class SignInQuery : IRequest<Response<UserResponse>>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
