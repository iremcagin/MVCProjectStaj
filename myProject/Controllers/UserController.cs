using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using myProject.Models;
using System.Collections.Generic;

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


        /* ------------------------------------------------------------------------------------------ */
        public IActionResult GetBasketCount()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(0); // Kullanıcı giriş yapmamışsa, sepet sayısı 0
            }

            int basketCount = userDatabaseControlModel.GetBasketCount(userId.Value);
            return Json(basketCount);
        }


        /* ------------------------------------------------------------------------------------------ */
        public IActionResult Signout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Guest");
        }



        /* ------------------------------------------------------------------------------------------ */
        /* Ürüne tıklanınca açılan sayfa */
        [HttpGet("User/ProductDetails/{productId}")]
        public IActionResult ProductDetails(int productId)
        {
            ProductDetailsModel productDetailsModel = new ProductDetailsModel();
            productDetailsModel = userDatabaseControlModel.ProductDetail(productId);


            ViewBag.ProductDetails = productDetailsModel;
            return View();
        }


        /* ------------------------------------------------------------------------------------------ */
        // Sepete ürün ekle
        [HttpPost]
        public IActionResult AddToCart(int productId, int companyId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            userDatabaseControlModel.AddToCart(userId, productId, companyId);
            return RedirectToAction("ProductDetails");
        }


        /* ------------------------------------------------------------------------------------------ */
        // Sepetteki ürünleri döndür

        public IActionResult Basket()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            List<ProductModel> allProducts = userDatabaseControlModel.GetBasket(userId);
            List<Tuple<int, string>> Images = new List<Tuple<int, string>>();
            Images = userDatabaseControlModel.GetProductImages(allProducts);


            ViewBag.allProductsInBasket = allProducts;
            ViewBag.ImagesOfProductsInBasket = Images;

            return View();
        }


    }
}
