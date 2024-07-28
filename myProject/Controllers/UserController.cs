using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using myProject.Models;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;



namespace myProject.Controllers
{
    [Authorize(Policy = "UserPolicy")]
    public class UserController : Controller
    {
        private readonly _UserDatabaseControlModel userDatabaseControlModel;



        public UserController(_UserDatabaseControlModel userDatabaseControlModel_)
        {
            userDatabaseControlModel = userDatabaseControlModel_;
        }



        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(0);
            }


            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages = userDatabaseControlModel.HomePageProducts();


            ModelForUserPages.companies = userDatabaseControlModel.GetAllCompanies();
            ModelForUserPages.productsLiked = userDatabaseControlModel.GetLikedProductIds(userId);


            return View(modelForUserPages);
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


            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(0);
            }
            ModelForUserPages.productsLiked = userDatabaseControlModel.GetLikedProductIds(userId);

            return View(modelForUserPages);
        }


        /* ------------------------------------------------------------------------------------------ */
        // Sepete ürün ekle
        [HttpPost]
        public IActionResult AddToCart(int productId, int companyId, int quantity)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            userDatabaseControlModel.AddToCart(userId, productId, companyId, quantity);

            return RedirectToAction("ProductDetails");
        }


        /* ------------------------------------------------------------------------------------------ */
        // Sepetteki ürünleri döndür

        public IActionResult Basket()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages = userDatabaseControlModel.GetBasket(userId);
            modelForUserPages.productsDeletedFromBasket = userDatabaseControlModel.GetPrevDeletedBasket(userId);
            ModelForUserPages.productsLiked = userDatabaseControlModel.GetLikedProductIds(userId);

            UpdatePrice();


            return View(modelForUserPages);
        }



        /* ------------------------------------------------------------------------------------------ */
        // Delete from basket
        [HttpGet("User/DeleteItemInBasket/{productId}")]
        public IActionResult DeleteItemInBasket(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }


            userDatabaseControlModel.DeleteItemInBasket(userId, productId);

            return RedirectToAction("Basket");
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Update quantity */
        public IActionResult UpdateQuantity(int productId, int companyId, int quantity)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

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
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ModelForUserPages.CalculatePrice(userId);

            return RedirectToAction("Basket");
        }



        /* -------------------------------------------------------------------------------------------------------------- */
        /* Get User Profile Info */
        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages = userDatabaseControlModel.GetUserProfileInfo(userId);
            ModelForUserPages.productsLiked = userDatabaseControlModel.GetLikedProductIds(userId);


            return View(modelForUserPages);
        }



        /* -------------------------------------------------------------------------------------------------------------- */
        /* Add Card */
        [HttpPost]
        public IActionResult AddCard(CardModel card)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            userDatabaseControlModel.AddCard(userId, card);

            return RedirectToAction("Profile");

        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Satın alım */
        [HttpPost]
        [HttpPost]
        public IActionResult ClearCart()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // Kullanıcı giriş yapmamışsa JSON formatında hata döndür
                return Json(new { success = false, message = "User not logged in" });
            }

            try
            {
                userDatabaseControlModel.ClearCart(userId);
                // Başarılı olduğunda JSON formatında başarı yanıtı döndür
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Hata durumunda JSON formatında hata yanıtı döndür
                return Json(new { success = false, message = ex.Message });
            }
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Review Ekleme */
        [HttpPost]
        public IActionResult AddReview(ProductReviewModel model)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }
            userDatabaseControlModel.AddReview(model, userId);


            return RedirectToAction("ProductDetails", new { productId = model.ProductId });
        }



        /* -------------------------------------------------------------------------------------------------------------- */
        /* Kategoriye göre ürün listeleme (filter) */
        public IActionResult ProductsByCategory(string categoryyy)
        {

            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages.subcategory = categoryyy;

            if (categoryyy == "Furniture") { modelForUserPages.productsByCategory = userDatabaseControlModel.GetSubcategoriesByMainCategory(categoryyy); }
            else if (categoryyy == "Decoration") { modelForUserPages.productsByCategory = userDatabaseControlModel.GetSubcategoriesByMainCategory(categoryyy); }
            else if (categoryyy == "all") modelForUserPages.productsByCategory = userDatabaseControlModel.getAllProducts();
            else { modelForUserPages.productsByCategory = userDatabaseControlModel.GetProductsByCategory(categoryyy); }


            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ModelForUserPages.productsLiked = userDatabaseControlModel.GetLikedProductIds(userId);

            return View(modelForUserPages);
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Şirketin sayfası */
        [HttpGet("User/CompanyDetails/{companyId}")]
        public IActionResult CompanyDetails(int companyId)
        {
            ModelForUserPages modelForUserPages = new ModelForUserPages();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            modelForUserPages = userDatabaseControlModel.CompanyDetails(companyId, userId);
            ModelForUserPages.productsLiked = userDatabaseControlModel.GetLikedProductIds(userId);

            return View(modelForUserPages);
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Satın alınan ürünlerin stoğunu azaltmak için */

        [HttpPost]
        public IActionResult PurchaseBasket(int[] productIds, int[] productQuantities)
        {
            if (productIds != null && productQuantities != null && productIds.Length == productQuantities.Length)
            {
                userDatabaseControlModel.PurchaseBasket(productIds, productQuantities);
            }
            else
            {
                Console.WriteLine("Invalid data received.");
            }

            return RedirectToAction("Basket");
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Follow company */
        [HttpGet]
        public IActionResult FollowUnfollowCompany(bool isFollowing, int companyId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (isFollowing)
            {
                // Unfollow the company
                userDatabaseControlModel.UnfollowCompany(userId, companyId);
            }
            else
            {
                // Follow the company
                userDatabaseControlModel.FollowCompany(userId, companyId);
            }

            return RedirectToAction("CompanyDetails", new { companyId });
        }



        /* -------------------------------------------------------------------------------------------------------------- */
        /* Like Button */
        [HttpGet("User/LikeButton/{productId}")]
        public IActionResult LikeButton(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }
            userDatabaseControlModel.LikeButton(userId, productId);


            // Redirect back to the referrer URL
            var referrerUrl = Request.Headers["Referer"].ToString();
            return Redirect(referrerUrl);
        }
        /* -------------------------------------------------------------------------------------------------------------- */
        /* Unlike Button */
        [HttpGet("User/UnlikeButton/{productId}")]
        public IActionResult UnlikeButton(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }
            userDatabaseControlModel.UnlikeButton(userId, productId);


            // Redirect back to the referrer URL
            var referrerUrl = Request.Headers["Referer"].ToString();
            return Redirect(referrerUrl);
        }

        /* -------------------------------------------------------------------------------------------------------------- */
        /* Favorites Page */
        public IActionResult Favorites(int productId)
        {
            ModelForUserPages modelForUserPages = new ModelForUserPages();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            modelForUserPages.favoriteProducts = userDatabaseControlModel.GetFavoriteProducts(userId);


            return View(modelForUserPages);
        }



    }
}
