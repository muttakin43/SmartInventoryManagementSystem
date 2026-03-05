using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Model
{
    public class Supplier : Entity
    {
        public string Name { get; set; }= string.Empty;
        public string Phone { get; set; }= string.Empty;

        public string Address { get; set; }= string.Empty;

        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    }
}
