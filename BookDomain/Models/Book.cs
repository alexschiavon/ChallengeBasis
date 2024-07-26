namespace BookDomain.Models
{
    public class Book
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public Guid EditionId { get; set; }
        public string PublicationYear { get; set; }
    }
}