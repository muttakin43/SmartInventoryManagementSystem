using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Contract.Request
{
    public class PurchaseUpdateRequest
    {
        public int SupplierId { get; set; }
        public DateTime PurchaseUpdateDate { get; set; }
        
        public List<PurchaseDetailsUpdateRequest> Items { get; set; }
    }
}
