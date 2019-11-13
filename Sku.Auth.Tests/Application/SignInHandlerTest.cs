using NSubstitute;
using Sky.Auth.Application.Handlers;
using Sky.Auth.Application.Queries;
using Sky.Auth.Domain.Interfaces;
using Sky.Auth.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Sky.Auth.Tests.Application
{
    public class SignInHandlerTest
    {
        private readonly IAuthRepository _authRepository;
        private readonly SignInHandler _handler;

        private const string NAME = "albino";
        private const string EMAIL = "binolima@gmail.com";
        private const string PASSWORD = "teste";
        private readonly List<Phone> PHONES = new List<Phone>() { new Phone("11", "999999999") };
        private const string OBJECT_ID = "123456";
        private const string TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjVkY2MwNmJiZjg1OWFhNWZkODdmMWI2ZCIsImVtYWlsIjoiYmlub2xpbWFAZ21haWwuY29tIiwibmJmIjoxNTczNjUyMzEyLCJleHAiOjE1NzQyNTcxMDYsImlhdCI6MTU3MzY1MjMxMn0.uqJdNDJDg5509uQVjPNZ69tJLuYTrylZ-BJyzd1lKQ8";

        public SignInHandlerTest()
        {
            _authRepository = Substitute.For<IAuthRepository>();
            _handler = new SignInHandler(_authRepository);
        }

        [Fact]
        public async Task Should_Returns_User_When_SuccessOnSingIn()
        {
            var command = new SignInQuery { Email = EMAIL, Password = PASSWORD };

            _authRepository.GetUserByEmail(Arg.Any<string>()).Returns(new User(OBJECT_ID, NAME, EMAIL, PASSWORD, PHONES, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, TOKEN));

            _authRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>()).Returns(new User(OBJECT_ID, NAME, EMAIL, PASSWORD, PHONES, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, TOKEN));

            _authRepository.UpdateLastLogin(Arg.Any<User>()).Returns(new User(OBJECT_ID, NAME, EMAIL, PASSWORD, PHONES, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, TOKEN));

            var actual = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(actual);
            Assert.True(actual.IsSuccess);
            Assert.NotNull(actual.Value.User);
            Assert.Equal(actual.Value.User.Email, EMAIL);
            Assert.Equal(actual.Value.User.Name, NAME);
        }

        [Fact]
        public async Task Should_Returns_BadRequest_When_EmailDoesNotExist()
        {
            var command = new SignInQuery { Email = EMAIL, Password = PASSWORD };

            _authRepository.GetUserByEmail(Arg.Any<string>()).Returns(Task.FromResult<User>(null));

            var actual = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(actual);
            Assert.True(actual.IsFailure);
        }

        [Fact]
        public async void Should_ThrowException_When_ConnectMongoDB_ReturnInvalidException()
        {
            var command = new SignInQuery { Email = EMAIL, Password = PASSWORD };

            _authRepository
                .When(x => x.GetUserByEmail(Arg.Any<string>()))
                .Do(callInfo => throw new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));
        }
    }
}
