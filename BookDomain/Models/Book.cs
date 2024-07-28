namespace BookDomain.Models
{
    public class Book
    {
        public Book()
        {
            BookId = Guid.Empty;
        }

        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int Edition { get; set; }
        public string PublicationYear { get; set; }
    }
}