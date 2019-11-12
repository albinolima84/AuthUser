using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sky.Auth.Application.Commands;
using Sky.Auth.Application.Queries;
using Sky.Auth.Application.Responses;

namespace Sky.Auth.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("signup")]
        [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SignUp([FromBody] CreateUserCommand userCommand)
        {
            if (userCommand == null)
            {
                return BadRequest("Parâmetros inválidos ou nulos");
            }

            var response = await _mediator.Send(userCommand);

            if (response.IsFailure)
            {
                return BadRequest(response.Messages);
            }

            return Ok(response.Value.User);
        }

        [HttpPost("signin")]
        [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SignIn([FromBody] GetUserQuery userQuery)
        {
            if (userQuery is null)
            {
                return BadRequest(CreateErrorResponse("Parâmetros inválidos ou nulos"));
            }

            var response = await _mediator.Send(userQuery);

            if (response.IsFailure)
            {
                return BadRequest(response.Messages);
            }

            if (response.Value?.User is null)
            {
                return BadRequest(CreateErrorResponse("Usuário e/ou senha inválidos"));
            }

            return Ok(response.Value.User);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUser([FromRoute] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Parâmetros inválidos ou nulos");
            }

            var response = await _mediator.Send(new GetUserByIdQuery { Id = userId, TokenHeader = string.Empty });

            if (response.IsFailure)
            {
                return BadRequest(response.Messages);
            }

            return Ok(response.Value.User);
        }

        private IDictionary<string, string> CreateErrorResponse(string message)
        {
            var errorMessage = new Dictionary<string, string>
            {
                { "message", message }
            };

            return errorMessage;
        }
    }
}
