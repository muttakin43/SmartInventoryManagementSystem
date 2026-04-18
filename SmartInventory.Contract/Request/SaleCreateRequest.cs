using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Contract.Request
{
    public class SaleCreateRequest
    {
        public int CustomerId { get; set; }
        public DateTime SaleDate {  get; set; }
        public decimal TotalAmount { get; set; }    

        public List<SaleDetailsRequest> SaleDetails { get; set; } = new List<SaleDetailsRequest>();
    }
}
