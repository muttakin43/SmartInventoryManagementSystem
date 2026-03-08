using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Contract.Request
{
    public class PurchaseCreateRequest
    {
        public int SupplierId { get; set; }

        public DateTime purchaseDate { get; set; }

        public List<PurchaseDetailsCreateRequest> Details { get; set; } = new List<PurchaseDetailsCreateRequest>();

    }
}
