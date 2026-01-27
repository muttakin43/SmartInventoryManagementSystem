

namespace SmartInventory.Model
{
    public class Product : Entity
    {
        public string ProductName { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }
         public int StockQuantit { get; set; }

        public int? CategoryId { get; set; }

        public Category? Category { get; set; }

    }
}
