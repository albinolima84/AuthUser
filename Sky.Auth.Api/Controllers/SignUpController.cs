using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sky.Auth.Command.Commands;
using Sky.Auth.Command.Dtos;

namespace Sky.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SignUpController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[ProducesResponseType(typeof(CreateCustomerWalletResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateUserCommand userCommand)
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

            return Ok(response.Value.CustomerWallet);
        }
    }
}
