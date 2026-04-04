namespace SmartInventory.Model
{
    public class Purchase : Entity
    {
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<PurchaseDetail> Details { get; set; } = new List<PurchaseDetail>();
    }
}