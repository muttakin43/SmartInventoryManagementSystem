using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Model
{
    public class StockTransaction : Entity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public string Type { get; set; } 
        public DateTime Date { get; set; }
    }
}
