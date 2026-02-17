using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SmartInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]

        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return Ok();
        }

        [HttpGet("{userId}")]

        public IActionResult GetById(int userId)
        {
            return Ok(userId);
        }

        [HttpGet("{userId}/orders")]

        public IActionResult GetOrdersByUserId(int userId)
        {
            return Ok(userId);
        }


        [HttpGet("{userId}/orders/{orderId}")]

        public IActionResult GetOrdersByUserId(int userId,int orderId) {
            return Ok(userId);
        }
    }
}
