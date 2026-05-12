using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartInventory.Contract.Request
{
    public class DashboardDto
    {
        public int TotalProducts { get; set; }
        public int TotalStock { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalPurchase { get; set; }
        public decimal Profit { get; set; }

        // Chart
        public List<string> Months { get; set; } = new();
        public List<decimal> SalesData { get; set; } = new();

       
    }
}
