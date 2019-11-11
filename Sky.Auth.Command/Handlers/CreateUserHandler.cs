using MediatR;
using Sky.Auth.Command.Commands;
using Sky.Auth.Command.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sky.Auth.Command.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        public Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
