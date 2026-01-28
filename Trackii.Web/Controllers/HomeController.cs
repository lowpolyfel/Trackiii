using Microsoft.AspNetCore.Mvc;

namespace Trackii.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
