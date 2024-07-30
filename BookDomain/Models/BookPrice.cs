namespace BookDomain.Models
{
    public class BookPrice
    {
        public Guid BookId { get; set; }
        public Guid PurchaseTypeId { get; set; }
        public decimal Price { get; set; }
    }
}
