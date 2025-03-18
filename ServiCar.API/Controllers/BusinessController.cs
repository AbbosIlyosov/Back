using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiCar.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class BusinessController : Controller
    {
        [HttpGet, Route("get-all")]
        public IActionResult GetAllBusinesses()
        {
            return Ok();
        }

        [HttpPost, Route("create"), Authorize(Roles = "Admin")]
        public IActionResult CreateBusiness()
        {
            return Ok();
        }

        [HttpPut, Route("update")]
        public IActionResult UpdateBusiness(int id)
        {
            return Ok();
        }

        [HttpPut, Route("update-status")]
        public IActionResult UpdateBusinessStatus(int id)
        {
            return Ok();
        }
    }
}
