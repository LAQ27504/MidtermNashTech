namespace LibraryManagement.Core.Application.DTOs.Responses
{
    public class PaginationResponse<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<T> Items { get; set; }
    }
}
