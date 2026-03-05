using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Contract.Request
{
    public class UpdateSupplierRequest
    {
        public int id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public string Phone { get; set; } = string.Empty;
      
        public string Address { get; set; } = string.Empty;
    }
}
