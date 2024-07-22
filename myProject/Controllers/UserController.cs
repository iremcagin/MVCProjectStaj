using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using myProject.Models;

namespace myProject.Controllers
{
    public class UserController : Controller
    {
        private readonly _UserDatabaseControlModel userDatabaseControlModel = new _UserDatabaseControlModel();


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


        public IActionResult Signout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Guest");
        }


        [HttpGet("User/ProductDetails/{productId}")]
        public IActionResult ProductDetails(int productId)
        {
            ProductDetailsModel productDetailsModel = new ProductDetailsModel();
            productDetailsModel = userDatabaseControlModel.ProductDetail(productId);
            ViewBag.ProductDetails = productDetailsModel;

            //Console.WriteLine(productDetailsModel.Product.Name);

            return View();
        }




    }
}
