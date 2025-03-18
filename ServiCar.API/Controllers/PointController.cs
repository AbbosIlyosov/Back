using Microsoft.AspNetCore.Mvc;

namespace ServiCar.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PointController : Controller
    {
        [HttpGet, Route("get-by-id")]
        public IActionResult GetPointById(int id)
        {
            return Ok("Hello World");
        }

        [HttpGet, Route("filtered")]
        public IActionResult GetPointsFiltered(int id)
        {
            return Ok($"Hello World {id}");
        }

        [HttpPost, Route("create")]
        public IActionResult CreatePoint(int id)
        {
            return Ok($"Hello World {id}");
        }

        [HttpPut, Route("update")]
        public IActionResult UpdatePoint(int id)
        {
            return Ok();
        }

        [HttpDelete, Route("delete")]
        public IActionResult DeletePoint(int id)
        {
            return Ok();
        }
    }
}
