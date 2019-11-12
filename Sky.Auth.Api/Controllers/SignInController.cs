using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sky.Auth.Application.Queries;
using Sky.Auth.Application.Responses;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Sky.Auth.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SignInController : Controller
    {
        private readonly IMediator _mediator;

        public SignInController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUser([FromBody] GetUserQuery userQuery)
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

            if(response.Value?.User is null)
            {
                return BadRequest(CreateErrorResponse("Usuário e/ou senha inválidos"));
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