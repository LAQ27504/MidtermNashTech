namespace LibraryManagement.Core.Application.DTOs.Responses
{
    public class BookResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string CategoryName { get; set; }

        public Guid CategoryId { get; set; }

        public int Amount { get; set; }

        public int AvailableAmount { get; set; }
    }
}
