using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Model
{
    public class Sale : Entity
    {
       
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public decimal TotalAmount { get; set; }

        public ICollection<SaleDetails> SaleDetails { get; set; } = new List<SaleDetails>();
    }
}
