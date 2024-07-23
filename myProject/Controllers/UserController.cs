using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using myProject.Models;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;

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
        [HttpGet]
        public IActionResult GetBasketCount()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(0);
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
            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages = userDatabaseControlModel.ProductDetail(productId);

            return View(modelForUserPages);
        }


        /* ------------------------------------------------------------------------------------------ */
        // Sepete ürün ekle
        [HttpPost]
        public IActionResult AddToCart(int productId, int companyId, int quantity)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            userDatabaseControlModel.AddToCart(userId, productId, companyId, quantity);

            return RedirectToAction("ProductDetails");
        }


        /* ------------------------------------------------------------------------------------------ */
        // Sepetteki ürünleri döndür

        public IActionResult Basket()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages.productsInBasket = userDatabaseControlModel.GetBasket(userId);
            modelForUserPages.productsDeletedFromBasket = userDatabaseControlModel.GetPrevDeletedBasket(userId);

            UpdatePrice();


            return View(modelForUserPages);
        }



        /* ------------------------------------------------------------------------------------------ */
        // Delete from basket
        [HttpGet("User/DeleteItemInBasket/{productId}")]
        public IActionResult DeleteItemInBasket(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            userDatabaseControlModel.DeleteItemInBasket(userId, productId);

            return RedirectToAction("Basket");
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Update quantity */
        public IActionResult UpdateQuantity(int productId, int companyId, int quantity)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            userDatabaseControlModel.updateQuantity(userId, productId, companyId, quantity);
            UpdatePrice();


            return RedirectToAction("Basket");
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Update price */
        [HttpGet]
        public IActionResult UpdatePrice()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            ModelForUserPages.CalculatePrice(userId);

            return RedirectToAction("Basket");
        }



        /* -------------------------------------------------------------------------------------------------------------- */
        /* Get User Profile Info */
        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages = userDatabaseControlModel.GetUserProfileInfo(userId);

            return View(modelForUserPages);
        }
    }
}
