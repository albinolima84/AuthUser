using MediatR;
using Sky.Auth.Application.Queries;
using Sky.Auth.Application.Responses;
using Sky.Auth.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Sky.Auth.Application.Handlers
{
    public class SignInHandler : IRequestHandler<SignInQuery, Response<UserResponse>>
    {
        private readonly IAuthRepository _authRepository;

        public SignInHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Response<UserResponse>> Handle(SignInQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var existUser = await _authRepository.GetUserByEmail(request.Email);
                if (existUser is null)
                {
                    return Response<UserResponse>.Fail("message", "Usuário e/ou senha inválidos");
                }

                var user = await _authRepository.Authenticate(request.Email, request.Password);

                if(user != null)
                    await _authRepository.UpdateLastLogin(user);

                return Response<UserResponse>.Ok(new UserResponse(user));
            }
            catch
            {
                throw;
            }
        }
    }
}
