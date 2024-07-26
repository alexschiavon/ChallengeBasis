namespace Arch.Domain.Adapters.Helper
{
    public class SortedBy 
    {
        public SortedBy()
        {
            Order = "asc";
        }
        public string? Field { get; set; }
        public string? Order { get; set; }
    }
}
