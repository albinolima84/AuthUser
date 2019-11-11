using MediatR;
using Sky.Auth.Command.Commands;
using Sky.Auth.Command.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sky.Auth.Command.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Response<CreateUserResponse>>
    {

        public CreateUserHandler()
        {

        }

        public Task<Response<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
