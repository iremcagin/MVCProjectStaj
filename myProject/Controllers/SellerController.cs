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
        private readonly SellerDatabaseControlModel databaseControlModel;



        /* --------------------------------------------------- Constructor --------------------------------------------------- */
        public SellerController()
        {
            databaseControlModel = new SellerDatabaseControlModel();
        }



        /* --------------------------------------------------- Dashboard Page --------------------------------------------------- */
        public IActionResult Index()
        {
            return View();
        }

        /* --------------------------------------------------- Products Page --------------------------------------------------- */
        public IActionResult Products(List<ProductModel> products_ = null, int page = 1, int pageSize = 7)
        {
            List<ProductModel> allProducts;
            
            if (products_ == null)
            {
                // Veri tabanından tüm ürünleri çek
                allProducts = databaseControlModel.getAllProducts();
            }
            else
            {
                // Arama sonucu ile gelen ürünleri kullan
                allProducts = products_;
            }

            
            // Sayfalama işlemleri
            var pagedProducts = allProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalPages = (int)Math.Ceiling((double)allProducts.Count / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.AllProducts = allProducts;

            return View(pagedProducts);  
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
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

    }
}
