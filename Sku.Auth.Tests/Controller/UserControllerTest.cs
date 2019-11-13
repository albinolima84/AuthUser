using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sky.Auth.Api.Controllers;
using Sky.Auth.Application.Commands;
using Sky.Auth.Application.Queries;
using Sky.Auth.Application.Responses;
using Sky.Auth.Domain.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sky.Auth.Tests.Controller
{
    public class UserControllerTest
    {
        private readonly IMediator _mediator;
        private readonly UserController _controller;

        private const string NAME = "albino";
        private const string EMAIL = "binolima@gmail.com";
        private const string PASSWORD = "teste";
        private readonly List<PhoneNumberCommand> PHONES_COMMAND = new List<PhoneNumberCommand>() { new PhoneNumberCommand("11", "999999999") };
        private readonly List<Phone> PHONES = new List<Phone>() { new Phone("11", "999999999") };
        private const string OBJECT_ID = "123456";
        private const string TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjVkY2MwNmJiZjg1OWFhNWZkODdmMWI2ZCIsImVtYWlsIjoiYmlub2xpbWFAZ21haWwuY29tIiwibmJmIjoxNTczNjUyMzEyLCJleHAiOjE1NzQyNTcxMDYsImlhdCI6MTU3MzY1MjMxMn0.uqJdNDJDg5509uQVjPNZ69tJLuYTrylZ-BJyzd1lKQ8";

        public UserControllerTest()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new UserController(_mediator);

            var identities = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, OBJECT_ID),
                    new Claim(ClaimTypes.Email, EMAIL)
                });

            _controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identities)
            };

            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = TOKEN;
        }

        [Fact]
        public async Task Should_Returns_BadRequest_When_CommandIsNull_OnSignUp()
        {
            var actual = await _controller.SignUp(null);

            Assert.NotNull(actual);
            Assert.Equal(((BadRequestObjectResult)actual).StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Returns_BadRequest_When_OccursFail_OnSignUp()
        {
            var command = new CreateUserCommand(NAME, EMAIL, PASSWORD, PHONES_COMMAND);

            _mediator.Send(Arg.Any<CreateUserCommand>()).Returns(x =>
            {
                return Response<UserResponse>.Fail("message", "FAIL");
            });

            var actual = await _controller.SignUp(command);

            Assert.NotNull(actual);
            Assert.Equal(((BadRequestObjectResult)actual).StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Returns_OK_When_Success_OnSignUp()
        {
            var command = new CreateUserCommand(NAME, EMAIL, PASSWORD, PHONES_COMMAND);

            _mediator.Send(Arg.Any<CreateUserCommand>()).Returns(x =>
            {
                return Response<UserResponse>.Ok(CreateResponse());
            });

            var actual = await _controller.SignUp(command);

            Assert.NotNull(actual);
            Assert.Equal(((OkObjectResult)actual).StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Returns_BadRequest_When_QueryIsNull_OnSignIn()
        {
            var actual = await _controller.SignIn(null);

            Assert.NotNull(actual);
            Assert.Equal(((BadRequestObjectResult)actual).StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Returns_BadRequest_When_OccursFail_OnSignIn()
        {
            var query = new SignInQuery { Email = EMAIL, Password = PASSWORD };

            _mediator.Send(Arg.Any<SignInQuery>()).Returns(x =>
            {
                return Response<UserResponse>.Fail("message", "FAIL");
            });

            var actual = await _controller.SignIn(query);

            Assert.NotNull(actual);
            Assert.Equal(((BadRequestObjectResult)actual).StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Returns_BadRequest_When_DoesNotExistUser_OnSignIn()
        {
            var query = new SignInQuery { Email = EMAIL, Password = PASSWORD };

            _mediator.Send(Arg.Any<SignInQuery>()).Returns(x =>
            {
                return Response<UserResponse>.Ok(CreateResponseNull());
            });

            var actual = await _controller.SignIn(query);

            Assert.NotNull(actual);
            Assert.Equal(((BadRequestObjectResult)actual).StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Returns_OK_When_SignInUser_OnSignIn()
        {
            var query = new SignInQuery { Email = EMAIL, Password = PASSWORD };

            _mediator.Send(Arg.Any<SignInQuery>()).Returns(x =>
            {
                return Response<UserResponse>.Ok(CreateResponse());
            });

            var actual = await _controller.SignIn(query);

            Assert.NotNull(actual);
            Assert.Equal(((OkObjectResult)actual).StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal(((User)((OkObjectResult)actual).Value).Password, PASSWORD);
            Assert.Equal(((User)((OkObjectResult)actual).Value).Email, EMAIL);
        }

        [Fact]
        public async Task Should_Returns_BadRequest_When_QueryIsNull_OnGetUser()
        {
            var actual = await _controller.GetUser(null);

            Assert.NotNull(actual);
            Assert.Equal(((BadRequestObjectResult)actual).StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Returns_NoContent_When_DoesNotFindUser_OnGetUser()
        {
            var query = new GetUserByIdQuery { Id = OBJECT_ID };

            _mediator.Send(Arg.Any<GetUserByIdQuery>()).Returns(x =>
            {
                return Response<UserResponse>.Ok(CreateResponseNull());
            });

            var actual = await _controller.GetUser(query);

            Assert.NotNull(actual);
            Assert.Equal(((NoContentResult)actual).StatusCode, (int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_Returns_Unauthorized_When_TokenIsInvalid_OnGetUser()
        {
            var query = new GetUserByIdQuery { Id = OBJECT_ID };

            _mediator.Send(Arg.Any<GetUserByIdQuery>()).Returns(x =>
            {
                return Response<UserResponse>.Ok(CreateResponse());
            });

            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "FAKE_TOKEN";

            var actual = await _controller.GetUser(query);

            Assert.NotNull(actual);
            Assert.Equal(((UnauthorizedResult)actual).StatusCode, (int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_Returns_BadRequest_When_SessionExpired_OnGetUser()
        {
            var query = new GetUserByIdQuery { Id = OBJECT_ID };

            _mediator.Send(Arg.Any<GetUserByIdQuery>()).Returns(x =>
            {
                return Response<UserResponse>.Ok(CreateResponseExpired());
            });

            var actual = await _controller.GetUser(query);

            Assert.NotNull(actual);
            Assert.Equal(((BadRequestObjectResult)actual).StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Returns_OK_When_GetUser()
        {
            var query = new GetUserByIdQuery { Id = OBJECT_ID };

            _mediator.Send(Arg.Any<GetUserByIdQuery>()).Returns(x =>
            {
                return Response<UserResponse>.Ok(CreateResponse());
            });

            var actual = await _controller.GetUser(query);

            Assert.NotNull(actual);
            Assert.Equal(((OkObjectResult)actual).StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal(((User)((OkObjectResult)actual).Value).Id, OBJECT_ID);
        }

        private UserResponse CreateResponseNull()
        {
            return new UserResponse(null);
        }

        private UserResponse CreateResponse()
        {
            var user = new User(OBJECT_ID, NAME, EMAIL, PASSWORD, PHONES, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, TOKEN);
            return new UserResponse(user);
        }

        private UserResponse CreateResponseExpired()
        {
            var user = new User(OBJECT_ID, NAME, EMAIL, PASSWORD, PHONES, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(-30), TOKEN);
            return new UserResponse(user);
        }
    }
}
