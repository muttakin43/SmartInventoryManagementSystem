using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Contract.Request
{
    public class UpdateProductRequest
    {
        public int id {  get; set; }
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Price {  get; set; }

        public int StockQuantit { get; set; }
    }
}
