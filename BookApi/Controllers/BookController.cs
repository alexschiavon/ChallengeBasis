using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{

    [Route("api/books")]
    [ApiController]
    public class BookController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Json(new { a = "a" });
        }
    }
}
