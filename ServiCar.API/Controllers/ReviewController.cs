using Microsoft.AspNetCore.Mvc;

namespace ServiCar.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ReviewController : Controller
    {
        [HttpGet, Route("filtered")]
        public IActionResult GetFiltered()
        {
            return Ok();
        }

        [HttpPost, Route("create")]
        public IActionResult CreateReview()
        {
            return Ok();
        }
    }
}
