namespace SmartInventory.Model
{
    public class PurchaseDetail : Entity
    {
        public int PurchaseId { get; set; }
        public Purchase? Purchase { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}