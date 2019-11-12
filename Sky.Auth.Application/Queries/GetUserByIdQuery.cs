using MediatR;
using Sky.Auth.Application.Responses;

namespace Sky.Auth.Application.Queries
{
    public class GetUserByIdQuery : IRequest<Response<UserResponse>>
    {
        public string Id { get; set; }

        public string TokenHeader { get; set; }
    }
}
