using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using myProject.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace myProject.Controllers
{
    public class SellerController : Controller
    {
        private readonly _SellerDatabaseControlModel databaseControlModel;

        /* --------------------------------------------------- Constructor --------------------------------------------------- */
        public SellerController()
        {
            databaseControlModel = new _SellerDatabaseControlModel();
        }

        /* --------------------------------------------------- Dashboard Page --------------------------------------------------- */
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var companyInfo = databaseControlModel.GetCompanyInfo(userId);
            if (companyInfo == null)
            {
                ViewBag.companyInfo = null;
            }
            else
            {
                ViewBag.companyInfo = companyInfo;
            }

            ModelForSellerPages modelForSellerPages = databaseControlModel.Dashboard(userId);

            return View(modelForSellerPages);
        }


        /* --------------------------------------------------- Products Page --------------------------------------------------- */
        public IActionResult Products(List<ProductModel> products_ = null, int page = 1, int pageSize = 7)
        {
            ModelForSellerPages modelForSellerPages = new ModelForSellerPages();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(0);
            }

            if (products_ == null)
            {
                modelForSellerPages.products = databaseControlModel.getAllProducts(userId);
            }
            else
            {
                modelForSellerPages.products = products_;
            }


            var companyInfo = databaseControlModel.GetCompanyInfo(userId);
            if (companyInfo == null)
            {
                ViewBag.companyInfo = null;
            }
            else
            {
                ViewBag.companyInfo = companyInfo;
            }



            modelForSellerPages.categories = databaseControlModel.getAllCategories();

            int totalProducts = modelForSellerPages.products?.Count ?? 0;
            var pagedProducts = new List<ProductModel>();

            if (totalProducts > 0)
            {
                pagedProducts = modelForSellerPages.products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;

            return View(modelForSellerPages);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProduct(List<IFormFile> images, ProductModel model)
        {
            try
            {
                string availability = model.isAvailable;
                model.isAvailable = availability == "available" ? "true" : "false";

                int? userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    return Json(0);
                }


                var companyInfo = databaseControlModel.GetCompanyInfo(userId);
                if (companyInfo == null)
                {
                    ViewBag.companyInfo = null;
                }
                else
                {
                    ViewBag.companyInfo = companyInfo;
                }


                foreach (var image in images)
                {
                    if (image != null && image.Length > 0)
                    {
                        // Kategoriye göre klasör oluşturma
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", model.Category);
                        Directory.CreateDirectory(uploadPath);

                        // Dosya adını al
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }
                        model.Images.Add(fileName);
                    }
                }
                databaseControlModel.saveProductToDatabase(model, userId);
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to add product: " + ex.Message;
                return RedirectToAction("Products");
            }
        }

        /* --------------------------------------------------- Navbar --------------------------------------------------- */
        public JsonResult Signout()
        {
            HttpContext.Session.Remove("UserId");
            return Json(new { success = true, message = "Signed out successfully" });
        }

        /* --------------------------------------------------- Orders Page --------------------------------------------------- */
        public IActionResult Orders(int page = 1, int pageSize = 7)
        {
            ModelForSellerPages modelForSellerPages = new ModelForSellerPages();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(0);
            }
            modelForSellerPages.orders = databaseControlModel.GetAllOrders(userId);

            int totalProducts = modelForSellerPages.orders?.Count ?? 0;
            var pagedProducts = new List<ProductModel>();
            if (totalProducts > 0)
            {
                pagedProducts = modelForSellerPages.orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;

            var companyInfo = databaseControlModel.GetCompanyInfo(userId);
            if (companyInfo == null)
            {
                ViewBag.companyInfo = null;
            }
            else
            {
                ViewBag.companyInfo = companyInfo;
            }

            return View(modelForSellerPages);
        }

        /* --------------------------------------------------- Customers Page --------------------------------------------------- */
        public IActionResult Customers(int page = 1, int pageSize = 10)
        {
            ModelForSellerPages modelForSellerPages = new ModelForSellerPages();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(0);
            }
            modelForSellerPages.customers = databaseControlModel.GetAllCustomers(userId);

            int totalProducts = modelForSellerPages.customers?.Count ?? 0;
            var pagedProducts = new List<UserModel>();
            if (totalProducts > 0)
            {
                pagedProducts = modelForSellerPages.customers.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;


            var companyInfo = databaseControlModel.GetCompanyInfo(userId);
            if (companyInfo == null)
            {
                ViewBag.companyInfo = null;
            }
            else
            {
                ViewBag.companyInfo = companyInfo;
            }

            return View(modelForSellerPages);
        }

        /* --------------------------------------------------- Followers Page --------------------------------------------------- */
        public IActionResult Followers(int page = 1, int pageSize = 10)
        {
            ModelForSellerPages modelForSellerPages = new ModelForSellerPages();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(0);
            }
            modelForSellerPages.followers = databaseControlModel.GetAllFollowers(userId);

            int totalProducts = modelForSellerPages.followers?.Count ?? 0;
            var pagedProducts = new List<UserModel>();
            if (totalProducts > 0)
            {
                pagedProducts = modelForSellerPages.followers.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;


            var companyInfo = databaseControlModel.GetCompanyInfo(userId);
            if (companyInfo == null)
            {
                ViewBag.companyInfo = null;
            }
            else
            {
                ViewBag.companyInfo = companyInfo;
            }

            return View(modelForSellerPages);
        }
    }
}
