using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Contract.Request
{
    public class UpdateCategoryRequest
    {
        public int id { get; set; }
        public string CategoryName { get; set; }= string.Empty;
        public string Description { get; set; }= string.Empty;


    }
}
