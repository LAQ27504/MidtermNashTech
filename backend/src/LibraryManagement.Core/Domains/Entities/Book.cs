namespace LibraryManagement.Core.Domains.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public Category BookCategory { get; set; }
    }
}
