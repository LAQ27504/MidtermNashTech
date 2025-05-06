namespace LibraryManagement.Core.Application.DTOs.Requests
{
    public class UserRegisterRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public UserType Type { get; set; }
    }
}
