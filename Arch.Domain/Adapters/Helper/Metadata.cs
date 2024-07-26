
namespace Arch.Domain.Adapters.Helper
{
    public class Metadata<T, Tf>
    {
        public List<T> Content { get; set; } = new List<T>();
        public Pagination Pagination { get; set; }
        public SortedBy SortedBy { get; set; }
        public List<string>? Warning { get; set; }
        public Tf Custom { get; set; }
    }

}
