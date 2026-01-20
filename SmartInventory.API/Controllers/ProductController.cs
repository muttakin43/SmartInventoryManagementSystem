
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Inteface;
using SmartInventory.Contract.Request;

namespace SmartInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetallAsync();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Error);
        }

        [HttpGet("{productId}")]

        public async Task<IActionResult> GetById(int productId)
        {
            var result = await _productService.GetByIdAsync(productId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(new { message = "Product not found" });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.AddAsync(request);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { productId = result.Data }, null);
            }
            return BadRequest(result.Error);
        }

        [HttpDelete("{productId}")]

        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _productService.DeleteAsync(productId);
            if (!result.Success)
            {
              if (result.Error.Contains("Not Found")==true)
                {
                    return NotFound(new { message = result.Error });
                }
                return BadRequest(result.Error);

            }
            return NoContent();

        }
        [HttpPut("{productId}")]
        public async  Task<IActionResult> Update(int productId , [FromBody] UpdateProductRequest request)
        {
           
            if (productId != request.id)
            {
                return BadRequest(new { message = "Product ID mismatch" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.UpdateAsync(request);
            if (!result.Success)
            {
                if (result.Error.Contains("Not Found") == true)
                {
                    return NotFound(new { message = result.Error });
                }
                return BadRequest(result.Error);
            }
            return CreatedAtAction(nameof(GetById),new { productId = result.Data },null);
        }


    }
}