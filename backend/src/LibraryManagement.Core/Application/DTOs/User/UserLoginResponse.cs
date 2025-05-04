namespace LibraryManagement.Core.Application.DTOs.Responses
{
    public class UserLoginResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
