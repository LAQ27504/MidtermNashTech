using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Application.Service;
using LibraryManagement.Core.Application.Service.Security;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace LibraryManagement.Test.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepo> _userRepo;
        private Mock<IUnitOfWork> _uow;
        private JwtService _jwtService;
        private UserService _svc;
        private User _existingUser;

        [SetUp]
        public void Setup()
        {
            // Build in-memory configuration for JwtService
            var inMemorySettings = new Dictionary<string, string>
            {
                { "JwtConfig:Key", "ThisIsASecretKeyForTests123!" },
                { "JwtConfig:Issuer", "TestIssuer" },
                { "JwtConfig:Audience", "TestAudience" },
                { "JwtConfig:TokenValidityMins", "60" },
            };
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Instantiate JwtService with config
            _jwtService = new JwtService(config);

            // Mock dependencies
            _userRepo = new Mock<IUserRepo>();
            _uow = new Mock<IUnitOfWork>();

            // UnitOfWork stubs
            _uow.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _uow.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
            _uow.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);
            _uow.Setup(x => x.RollbackAsync()).Returns(Task.CompletedTask);

            // Create service
            _svc = new UserService(_userRepo.Object, _uow.Object, _jwtService);

            // Seed an existing user for login tests
            _existingUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "john",
                Type = UserType.NormalUser,
                HashedPassword = string.Empty,
            };
            _existingUser.HashedPassword = PasswordService.HashPassword(
                _existingUser,
                "password123"
            );

            _userRepo.Setup(x => x.GetUserByName("john")).ReturnsAsync(_existingUser);
        }

        // AddUserExecute tests

        [Test]
        public async Task AddUser_WhenNameEmpty_Fails()
        {
            var req = new UserRegisterRequest
            {
                Name = "",
                Password = "password",
                ConfirmPassword = "password",
                Type = UserType.NormalUser,
            };

            var res = await _svc.AddUserExecute(req);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("Name and password are required", res.Message);
        }

        [Test]
        public async Task AddUser_WhenPasswordEmpty_Fails()
        {
            var req = new UserRegisterRequest
            {
                Name = "alice",
                Password = "",
                ConfirmPassword = "",
                Type = UserType.NormalUser,
            };

            var res = await _svc.AddUserExecute(req);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("Name and password are required", res.Message);
        }

        [Test]
        public async Task AddUser_WhenPasswordTooShort_Fails()
        {
            var req = new UserRegisterRequest
            {
                Name = "alice",
                Password = "123",
                ConfirmPassword = "123",
                Type = UserType.NormalUser,
            };

            var res = await _svc.AddUserExecute(req);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("Password must be at least 6 characters long", res.Message);
        }

        [Test]
        public async Task AddUser_WhenTypeInvalid_Fails()
        {
            var req = new UserRegisterRequest
            {
                Name = "alice",
                Password = "password123",
                ConfirmPassword = "password123",
                Type = (UserType)999,
            };

            var res = await _svc.AddUserExecute(req);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("Invalid user type", res.Message);
        }

        [Test]
        public async Task AddUser_WhenPasswordsDoNotMatch_Fails()
        {
            var req = new UserRegisterRequest
            {
                Name = "alice",
                Password = "password123",
                ConfirmPassword = "different",
                Type = UserType.NormalUser,
            };

            var res = await _svc.AddUserExecute(req);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("Passwords do not match", res.Message);
        }

        [Test]
        public async Task AddUser_WhenNameTooShort_Fails()
        {
            var req = new UserRegisterRequest
            {
                Name = "al",
                Password = "password123",
                ConfirmPassword = "password123",
                Type = UserType.NormalUser,
            };

            var res = await _svc.AddUserExecute(req);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("Name must be at least 3 characters long", res.Message);
        }

        [Test]
        public async Task AddUser_WhenUserExists_Fails()
        {
            // "john" already exists
            var req = new UserRegisterRequest
            {
                Name = "john",
                Password = "password123",
                ConfirmPassword = "password123",
                Type = UserType.NormalUser,
            };

            var res = await _svc.AddUserExecute(req);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("User already exists", res.Message);
        }

        // [Test]
        // public async Task AddUser_WhenValid_CreatesAndReturnsLoginResponse()
        // {
        //     // setup new user scenario
        //     _userRepo.Setup(x => x.GetUserByName("bob")).ReturnsAsync((User)null);

        //     var req = new UserRegisterRequest
        //     {
        //         Name = "bob",
        //         Password = "password123",
        //         ConfirmPassword = "password123",
        //         Type = UserType.NormalUser,
        //     };

        //     var res = await _svc.AddUserExecute(req);

        //     Assert.IsTrue(res.Success);
        //     Assert.IsInstanceOf<UserLoginResponse>(res.Data);

        //     var loginRes = (UserLoginResponse)res.Data!;
        //     Assert.AreEqual("bob", loginRes.Name);
        //     Assert.IsNotEmpty(loginRes.AccessToken);
        //     Assert.Greater(loginRes.ExpiresIn, 0);

        //     _userRepo.Verify(x => x.AddAsync(It.Is<User>(u => u.Name == "bob")), Times.Once);
        //     _uow.Verify(x => x.CommitAsync(), Times.Once);
        // }

        // LoginUserExecute tests

        [Test]
        public async Task LoginUser_WhenUserNotFound_Fails()
        {
            _userRepo.Setup(x => x.GetUserByName("nobody")).ReturnsAsync((User)null);

            var res = await _svc.LoginUserExecute(
                new UserLoginRequest { Name = "nobody", Password = "whatever" }
            );

            Assert.IsFalse(res.Success);
            Assert.AreEqual("User not found", res.Message);
        }

        [Test]
        public async Task LoginUser_WhenInvalidPassword_Fails()
        {
            var res = await _svc.LoginUserExecute(
                new UserLoginRequest { Name = "john", Password = "wrong" }
            );

            Assert.IsFalse(res.Success);
            Assert.AreEqual("Invalid password", res.Message);
        }

        // [Test]
        // public async Task LoginUser_WhenValid_ReturnsUserLoginResponse()
        // {
        //     var res = await _svc.LoginUserExecute(
        //         new UserLoginRequest { Name = "john", Password = "password123" }
        //     );

        //     Assert.IsTrue(res.Success);
        //     Assert.IsInstanceOf<UserLoginResponse>(res.Data);

        //     var data = (UserLoginResponse)res.Data!;
        //     Assert.AreEqual("john", data.Name);
        //     Assert.AreEqual(_existingUser.Id, data.Id);
        //     Assert.IsNotEmpty(data.AccessToken);
        //     Assert.Greater(data.ExpiresIn, 0);
        // }
    }
}
