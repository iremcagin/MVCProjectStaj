using Microsoft.AspNetCore.Mvc;
using myProject.Models;

namespace myProject.Controllers
{
    public class GuestController : Controller
    {
        private readonly CategoriesModel _categoriesRepository;

        public GuestController()
        {
            _categoriesRepository = new CategoriesModel();
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
