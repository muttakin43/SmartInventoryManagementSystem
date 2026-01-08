using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Inteface;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;

namespace SmartInventory.web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }



        public IActionResult Index()
        {
          
            return View();
        }


    }

}
