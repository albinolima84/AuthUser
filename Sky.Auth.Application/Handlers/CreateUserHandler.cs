using MediatR;
using Sky.Auth.Application.Commands;
using Sky.Auth.Application.Extensions;
using Sky.Auth.Application.Responses;
using Sky.Auth.Domain.Interfaces;
using Sky.Auth.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Sky.Auth.Application.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Response<UserResponse>>
    {
        private readonly IAuthRepository _authRepository;

        public CreateUserHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Response<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existUser = await _authRepository.GetUserByEmail(request.Email);
                if(existUser != null)
                {
                    return Response<UserResponse>.Fail("message","E-mail já existente");
                }

                var insertedUser = await _authRepository.CreateUser(new User(string.Empty, request.Name, request.Email, request.Password, request.PhoneNumbers.ToDomain()));
                return Response<UserResponse>.Ok(new UserResponse(insertedUser));
            }
            catch
            {
                throw;
            }
        }
    }
}
