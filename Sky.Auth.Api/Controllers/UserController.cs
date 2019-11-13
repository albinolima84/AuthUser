using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sky.Auth.Api.Helpers;
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
        public async Task<IActionResult> SignIn([FromBody] SignInQuery userQuery)
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

        [Authorize, HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUser([FromRoute] GetUserByIdQuery userId)
        {
            if (userId is null)
            {
                return BadRequest(CreateErrorResponse("Parâmetros inválidos ou nulos"));
            }

            var tokenEmail = User.GetUserEmail();
            var tokenUserId = User.GetUserId();
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var response = await _mediator.Send(userId);

            var user = response.Value?.User;

            if (user is null)
            {
                return NoContent();
            }

            if(token != user.Token || user.Email != tokenEmail || user.Id != tokenUserId)
            {
                return Unauthorized();
            }

            if((DateTime.UtcNow - user.LastLogin).TotalMinutes >= 30)
            {
                return BadRequest(CreateErrorResponse("Sessão inválida"));
            }

            return Ok(user);
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
