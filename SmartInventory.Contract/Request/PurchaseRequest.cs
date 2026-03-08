using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Contract.Request
{
    public class PurchaseRequest
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;

            public DateTime PurchaseDate { get; set; }
            public decimal TotalAmount { get; set; }
    }
}
