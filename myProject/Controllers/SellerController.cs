using Microsoft.AspNetCore.Mvc;

namespace myProject.Controllers
{
    public class SellerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
