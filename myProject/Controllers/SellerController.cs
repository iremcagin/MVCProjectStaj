using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using myProject.Models;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Web;

namespace myProject.Controllers
{
    public class SellerController : Microsoft.AspNetCore.Mvc.Controller
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

            return View();
        }

        /* --------------------------------------------------- Products Page --------------------------------------------------- */
        public IActionResult Products(List<ProductModel> products_ = null, int page = 1, int pageSize = 7)
        {
            ModelForSellerPages modelForSellerPages = new ModelForSellerPages();
            
            if (products_ == null)
            {
                // Veri tabanından tüm ürünleri çek
                modelForSellerPages.products = databaseControlModel.getAllProducts();
            }
            else
            {
                modelForSellerPages.products = products_;
            }

            modelForSellerPages.categories = databaseControlModel.getAllCategories();

            
            // Sayfalama işlemleri
            var pagedProducts = modelForSellerPages.products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalPages = (int)Math.Ceiling((double)modelForSellerPages.products.Count / pageSize);
            ViewBag.CurrentPage = page;

            return View(modelForSellerPages);  
        }


        [HttpPost]
        public async Task<IActionResult> AddNewProduct(List<IFormFile> images, ProductModel model)
        {
            try
            {
                string availability = model.isAvailable;
                if (availability == "available") model.isAvailable = "true";
                else model.isAvailable = "false";   


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
                databaseControlModel.saveProductToDatabase(model);
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to add product: " + ex.Message;
                return RedirectToAction("Products");
            }
        }



        /* --------------------------------------------------- Navbar --------------------------------------------------- */
        public IActionResult Signout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Guest");
        }


    }
}
