using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.DTOs.Responses;

namespace LibraryManagement.Core.Application.Interface.Services
{
    public interface IUserService
    {
        Task<OperationResult> AddUserExecute(UserRegisterRequest userRegister);

        Task<OperationResult> LoginUserExecute(UserLoginRequest userLogin);
    }
}
