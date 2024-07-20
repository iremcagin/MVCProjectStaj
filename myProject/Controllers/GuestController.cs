using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using System.Diagnostics;

namespace myProject.Controllers
{
    public class GuestController : Controller
    {
        /*
        private readonly CategoriesModel _categoriesRepository;

        public GuestController()
        {
            _categoriesRepository = new CategoriesModel();
        } */

        private readonly UserDatabaseControlModel userDatabaseControlModel = new UserDatabaseControlModel();


        public IActionResult Index()
        {
            List<ProductModel> products = userDatabaseControlModel.getAllProducts();
            List<ProductModel> mostClickedProducts = userDatabaseControlModel.GetMostClickedProducts();
            List<ProductModel> newestProducts = userDatabaseControlModel.GetNewestProducts();



            // ViewBag kullanarak view'a veri gönderme
            ViewBag.Products = products;
            ViewBag.MostClickedProducts = mostClickedProducts;
            ViewBag.NewestProducts = newestProducts;

          
          

            return View();
        }


    }
}
