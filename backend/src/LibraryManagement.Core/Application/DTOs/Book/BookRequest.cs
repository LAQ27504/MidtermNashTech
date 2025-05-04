namespace LibraryManagement.Core.Application.DTOs.Requests
{
    public class BookRequest
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public Guid CategoryId { get; set; }

        public int Amount { get; set; }
    }
}
