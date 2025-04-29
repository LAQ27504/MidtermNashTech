namespace LibraryManagement.Core.Domains.Entities
{
    public class Category
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
