using System;
using System.Threading.Tasks;
using LibraryManagement.API.Controllers;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Services;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LibraryManagement.Test.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private AccountController _controller;

        private UserRegisterRequest _registerRequest;
        private UserLoginRequest _loginRequest;
        private UserLoginResponse _loginResponse;

        [SetUp]
        public void SetUp()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new AccountController(_userServiceMock.Object);

            _registerRequest = new UserRegisterRequest
            {
                Name = "testuser",
                Password = "P@ssw0rd",
                ConfirmPassword = "P@ssw0rd",
                Type = UserType.SuperUser,
            };

            _loginRequest = new UserLoginRequest { Name = "testuser", Password = "P@ssw0rd" };

            _loginResponse = new UserLoginResponse
            {
                Id = Guid.NewGuid(),
                Name = "testuser",
                AccessToken = "fake-jwt-token",
                ExpiresIn = 3600,
            };
        }

        [Test]
        public async Task Register_Success_ReturnsOk()
        {
            _userServiceMock
                .Setup(x => x.AddUserExecute(_registerRequest))
                .ReturnsAsync(OperationResult.Ok(new { Message = "Registered successfully" }));

            var result = await _controller.Register(_registerRequest) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task Register_Failure_ReturnsBadRequest()
        {
            _userServiceMock
                .Setup(x => x.AddUserExecute(_registerRequest))
                .ReturnsAsync(OperationResult.Fail("Passwords do not match"));

            var result = await _controller.Register(_registerRequest) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Passwords do not match", result.Value);
        }

        [Test]
        public async Task Login_Success_ReturnsOk()
        {
            _userServiceMock
                .Setup(x => x.LoginUserExecute(_loginRequest))
                .ReturnsAsync(OperationResult.Ok(_loginResponse, "Login successful"));

            var result = await _controller.Login(_loginRequest) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            dynamic response = result.Value;
            Assert.AreEqual("Login successful", response.Message);
        }

        [Test]
        public async Task Login_Failure_ReturnsUnauthorized()
        {
            _userServiceMock
                .Setup(x => x.LoginUserExecute(_loginRequest))
                .ReturnsAsync(OperationResult.Fail("Invalid credentials"));

            var result = await _controller.Login(_loginRequest) as UnauthorizedObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(401, result.StatusCode);
            Assert.AreEqual("Invalid credentials", result.Value);
        }
    }
}
