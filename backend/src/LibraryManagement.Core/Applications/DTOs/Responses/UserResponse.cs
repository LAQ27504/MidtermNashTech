namespace LibraryManagement.Core.Application.DTOs.Responses
{
    public class UserResponse
    {
        public string Name { get; set; }

        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
