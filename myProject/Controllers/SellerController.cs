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
        private List<ProductModel> products = new List<ProductModel>
    {
        
            new ProductModel
            {
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.00m,
                Stock = 10,
                CreatedAt = DateTime.Now,
                Category = "Category 1",
                Rating = 4.5f,
                Favorite = 1,
                isAvailable = "false",
                Images = new List<string> { "koltuk1.webp", "koltuk1_2.webp", "logo.png","user.jpg"},
                Reviews = new List<ProductReviewModel> {
                    new ProductReviewModel { ReviewId = 1, ProductId = 1, UserId = 1, Rating = 5, Review="çok güzel.", CreatedAt = DateTime.Now},
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="mükemmel." ,CreatedAt = DateTime.Now.AddDays(-1) },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="bir beden büyük almanızı öneririm." ,CreatedAt = DateTime.Now.AddDays(-1) },

                }
            },
             new ProductModel
            {
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.00m,
                Stock = 10,
                CreatedAt = DateTime.Now,
                Category = "Category 1",
                Rating = 3f,
                Favorite = 0,
                Images = new List<string> { "koltuk1.webp", "koltuk1_2.webp", "logo.png","user.jpg"},
                Reviews = new List<ProductReviewModel> {
                    new ProductReviewModel { ReviewId = 1, ProductId = 1, UserId = 1, Rating = 5, Review="çok güzel.", CreatedAt = DateTime.Now },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="mükemmel." ,CreatedAt = DateTime.Now.AddDays(-1) },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="bir beden büyük almanızı öneririm." ,CreatedAt = DateTime.Now.AddDays(-1) },

                }
            },
              new ProductModel
            {
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.00m,
                Stock = 10,
                CreatedAt = DateTime.Now,
                Category = "Category 1",
                Rating = 3.5f,
                Favorite = 0,
                Images = new List<string> { "koltuk1.webp", "koltuk1_2.webp", "logo.png","user.jpg"},
                Reviews = new List<ProductReviewModel> {
                    new ProductReviewModel { ReviewId = 1, ProductId = 1, UserId = 1, Rating = 5, Review="çok güzel.", CreatedAt = DateTime.Now },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="mükemmel." ,CreatedAt = DateTime.Now.AddDays(-1) },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="bir beden büyük almanızı öneririm." ,CreatedAt = DateTime.Now.AddDays(-1) },

                }
            },
               new ProductModel
            {
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.00m,
                Stock = 10,
                CreatedAt = DateTime.Now,
                Category = "Category 1",
                Rating = 2f,
                Favorite = 1,
                Images = new List<string> { "koltuk1.webp", "koltuk1_2.webp", "logo.png","user.jpg"},
                Reviews = new List<ProductReviewModel> {
                    new ProductReviewModel { ReviewId = 1, ProductId = 1, UserId = 1, Rating = 5, Review="çok güzel.", CreatedAt = DateTime.Now },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="mükemmel." ,CreatedAt = DateTime.Now.AddDays(-1) },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="bir beden büyük almanızı öneririm." ,CreatedAt = DateTime.Now.AddDays(-1) },

                }
            },
                new ProductModel
            {
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.00m,
                Stock = 10,
                CreatedAt = DateTime.Now,
                Category = "Category 1",
                Rating = 1.5f,
                Favorite = 1,
                Images = new List<string> { "koltuk1.webp", "koltuk1_2.webp", "logo.png","user.jpg"},
                Reviews = new List<ProductReviewModel> {
                    new ProductReviewModel { ReviewId = 1, ProductId = 1, UserId = 1, Rating = 5, Review="çok güzel.", CreatedAt = DateTime.Now },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="mükemmel." ,CreatedAt = DateTime.Now.AddDays(-1) },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="bir beden büyük almanızı öneririm." ,CreatedAt = DateTime.Now.AddDays(-1) },

                }
            },
                 new ProductModel
            {
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.00m,
                Stock = 10,
                CreatedAt = DateTime.Now,
                Category = "Category 1",
                Rating = 5,
                Favorite = 1,
                Images = new List<string> { "koltuk1.webp", "koltuk1_2.webp", "logo.png","user.jpg"},
                Reviews = new List<ProductReviewModel> {
                    new ProductReviewModel { ReviewId = 1, ProductId = 1, UserId = 1, Rating = 5, Review="çok güzel.", CreatedAt = DateTime.Now },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="mükemmel." ,CreatedAt = DateTime.Now.AddDays(-1) },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="bir beden büyük almanızı öneririm." ,CreatedAt = DateTime.Now.AddDays(-1) },

                }
            },
                  new ProductModel
            {
                Name = "Product 1",
                Description = "Description 1",
                Price = 100.00m,
                Stock = 10,
                CreatedAt = DateTime.Now,
                Category = "Category 1",
                Rating = 5,
                Favorite = 1,
                Images = new List<string> { "koltuk1.webp", "koltuk1_2.webp", "logo.png","user.jpg"},
                Reviews = new List<ProductReviewModel> {
                    new ProductReviewModel { ReviewId = 1, ProductId = 1, UserId = 1, Rating = 5, Review="çok güzel.", CreatedAt = DateTime.Now },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="mükemmel." ,CreatedAt = DateTime.Now.AddDays(-1) },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="bir beden büyük almanızı öneririm." ,CreatedAt = DateTime.Now.AddDays(-1) },

                },
            },
                         new ProductModel
            {
                Name = "test",
                Description = "Black table for six people and can bigger",
                Price = 100.00m,
                Stock = 10,
                CreatedAt = DateTime.Now,
                Category = "Category 1",
                Rating = 5,
                Favorite = 1,
                Images = new List<string> { "koltuk1.webp", "koltuk1_2.webp", "logo.png","user.jpg"},
                Reviews = new List<ProductReviewModel> {
                    new ProductReviewModel { ReviewId = 1, ProductId = 1, UserId = 1, Rating = 5, Review="çok güzel.", CreatedAt = DateTime.Now },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="mükemmel." ,CreatedAt = DateTime.Now.AddDays(-1) },
                    new ProductReviewModel { ReviewId = 2, ProductId = 1, UserId = 2, Rating = 4, Review="bir beden büyük almanızı öneririm." ,CreatedAt = DateTime.Now.AddDays(-1) },

                }
                         }

        // Add more products as needed
        };

        /* Dashboard */
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
                //allProducts = _context.Products.ToList();
                allProducts = products; // initial list 
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
            {
                
                var newProduct = new ProductModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Stock = model.Stock,
                    CreatedAt = DateTime.Now,
                    Category = model.Category,
                    Rating = 0,
                    Favorite = 0,
                    isAvailable = model.isAvailable,
                    Reviews = new List<ProductReviewModel>(),
                    Images = new List<string>()
                };


                
                foreach (var image in images)
                {
                    if (image != null && image.Length > 0)
                    {
                        // Kategoriye göre klasör oluşturma
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", newProduct.Category);
                        Directory.CreateDirectory(uploadPath);

                        // Dosya adını al
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        newProduct.Images.Add(fileName);
                    }
                }
                
                ProductModel.saveData(newProduct);
                return RedirectToAction("Products");


            }
        }

    }
}
