namespace BookDomain.Models.Report
{
    public class BookDetails
    {
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public string Publisher { get; set; }
        public int Edition { get; set; }
        public string PublicationYear { get; set; }
        public string Authors { get; set; }
        public string Subjects { get; set; }
        public string Prices { get; set; }
    }
}
