using Microsoft.AspNetCore.Mvc;

namespace FashionSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}