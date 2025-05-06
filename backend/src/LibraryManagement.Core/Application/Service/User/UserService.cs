using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Application.Interface.Services;
using LibraryManagement.Core.Application.Service.Security;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        private readonly JwtService _jwtService;

        public UserService(IUserRepo userRepo, IUnitOfWork unitOfWork, JwtService jwtService)
        {
            _jwtService = jwtService;
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> AddUserExecute(UserRegisterRequest userRegister)
        {
            var isUserExist = await _userRepo.GetUserByName(userRegister.Name) != null;
            if (isUserExist)
            {
                return OperationResult.Fail("User already exists");
            }

            if (
                string.IsNullOrEmpty(userRegister.Name)
                || string.IsNullOrEmpty(userRegister.Password)
            )
            {
                return OperationResult.Fail("Name and password are required");
            }
            if (userRegister.Password.Length < 6)
            {
                return OperationResult.Fail("Password must be at least 6 characters long");
            }
            if (userRegister.Type != UserType.SuperUser && userRegister.Type != UserType.NormalUser)
            {
                return OperationResult.Fail("Invalid user type");
            }

            if (userRegister.Password != userRegister.ConfirmPassword)
            {
                return OperationResult.Fail("Passwords do not match");
            }
            if (userRegister.Name.Length < 3)
            {
                return OperationResult.Fail("Name must be at least 3 characters long");
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                HashedPassword = string.Empty,
                Name = userRegister.Name,
                Type = userRegister.Type,
            };
            newUser.HashedPassword = PasswordService.HashPassword(newUser, userRegister.Password);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _userRepo.AddAsync(newUser);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                var userLogin = new UserLoginRequest
                {
                    Name = newUser.Name,
                    Password = userRegister.Password,
                };

                return await LoginUserExecute(userLogin);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail("Failed to create user");
            }
        }

        public async Task<OperationResult> LoginUserExecute(UserLoginRequest userLogin)
        {
            var user = await _userRepo.GetUserByName(userLogin.Name);
            if (user == null)
            {
                return OperationResult.Fail("User not found");
            }

            var isPasswordValid = PasswordService.VerifyPassword(user, userLogin.Password);
            if (!isPasswordValid)
            {
                return OperationResult.Fail("Invalid password");
            }

            var token = _jwtService.GenerateToken(user.Id, user.Name, user.Type);

            return OperationResult.Ok(token, "Login successful");
        }
    }
}
