using Microsoft.AspNetCore.Mvc;

namespace Pacagroup.Ecommerce.Services.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(System.DateTime.Now);
        }
    }
}
