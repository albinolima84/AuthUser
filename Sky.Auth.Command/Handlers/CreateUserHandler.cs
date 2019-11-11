using MediatR;
using Sky.Auth.Command.Commands;
using Sky.Auth.Command.Extensions;
using Sky.Auth.Command.Responses;
using Sky.Auth.Domain.Interfaces;
using Sky.Auth.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sky.Auth.Command.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Response<CreateUserResponse>>
    {
        private readonly IAuthRepository _authRepository;

        public CreateUserHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Response<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existUser = await _authRepository.GetUserByEmail(request.Email);
                if(!string.IsNullOrEmpty(existUser.Id))
                {
                    return Response<CreateUserResponse>.Fail("ExistUser","E-mail já existente");
                }

                var insertedUser = await _authRepository.CreateUser(new User(string.Empty, request.Name, request.Email, request.Password, request.PhoneNumbers.ToDomain()));
                return Response<CreateUserResponse>.Ok(new CreateUserResponse(insertedUser));
            }
            catch
            {
                throw;
            }
        }
    }
}
