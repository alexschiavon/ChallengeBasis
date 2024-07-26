namespace Arch.Domain.Adapters.Helper
{
    public class Pagination
    {
        public int Limit { get; set; }
        public int CurrentPage { get; set; }
        public int? PageCount { get; set; }
        public int? TotalCount { get; set; }
    }
}
