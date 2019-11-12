using MediatR;
using Sky.Auth.Application.Queries;
using Sky.Auth.Application.Responses;
using Sky.Auth.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Sky.Auth.Application.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Response<UserResponse>>
    {
        private readonly IAuthRepository _authRepository;

        public GetUserByIdHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Response<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _authRepository.GetUserById(request.Id);
                if (user is null)
                {
                    return Response<UserResponse>.Fail("message", "Usuário e/ou senha inválidos");
                }

                return Response<UserResponse>.Ok(new UserResponse(user));
            }
            catch
            {
                throw;
            }
        }
    }
}
