namespace Aasaan_API.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public long TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
    }
}
