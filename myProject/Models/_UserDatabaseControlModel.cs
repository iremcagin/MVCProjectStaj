using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using NuGet.Protocol.Plugins;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;

namespace myProject.Models
{
    public class _UserDatabaseControlModel
    {

        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\iremc\OneDrive\Documents\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30";


        public _UserDatabaseControlModel()
        {


        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return all products */
        public List<ProductModel> getAllProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string productQuery = "SELECT * FROM Products";
                    using (SqlCommand cmd = new SqlCommand(productQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductModel product = new ProductModel
                                {
                                    ProductId = (int)reader["ProductId"],
                                    CompanyId = (int)reader["CompanyId"],
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = (decimal)reader["Price"],
                                    Stock = (int)reader["Stock"],
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    Category = reader["Category"].ToString(),
                                    Rating = Convert.ToSingle(reader["Rating"]),  // Float tipi için Convert.ToSingle
                                    Favorite = (int)reader["Favorite"],
                                    isAvailable = reader["isAvailable"].ToString(),
                                    Images = new List<string>()
                                };
                                products.Add(product);
                            }
                        }
                    }
                }


                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string imageQuery = "SELECT ProductId, ImageURL FROM ProductImages";
                    using (SqlCommand cmd = new SqlCommand(imageQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) // Check if there are rows returned by the query
                            {
                                while (reader.Read())
                                {
                                    int productId = (int)reader["ProductId"];
                                    string imageUrl = reader["ImageURL"].ToString();

                                    // İlgili ürünü bul ve resim ekle
                                    ProductModel product = products.FirstOrDefault(p => p.ProductId == productId);
                                    if (product != null && !product.Images.Contains(imageUrl)) // Check if image already exists
                                    {
                                        product.Images.Add(imageUrl);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading products: " + ex.Message);
            }

            return products;
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return top most 5 viewed product. */
        public List<ProductModel> GetMostClickedProducts(int limit = 5)
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    string query = @"
                        SELECT TOP (@Limit) *
                        FROM Products 
                        ORDER BY Clicked DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Limit", limit);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductModel product = new ProductModel
                                {
                                    ProductId = (int)reader["ProductId"],
                                    CompanyId = (int)reader["CompanyId"],
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = (decimal)reader["Price"],
                                    Stock = (int)reader["Stock"],
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    Category = reader["Category"].ToString(),
                                    Rating = Convert.ToSingle(reader["Rating"]),  // Float tipi için Convert.ToSingle kullanın
                                    Favorite = (int)reader["Favorite"],
                                    isAvailable = reader["isAvailable"].ToString(),
                                    Images = new List<string>()
                                };
                                products.Add(product);
                            }
                        }
                    }
                }

                /* Read Images */
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string imageQuery = "SELECT ProductId, ImageURL FROM ProductImages";
                    using (SqlCommand cmd = new SqlCommand(imageQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) // Check if there are rows returned by the query
                            {
                                while (reader.Read())
                                {
                                    int productId = (int)reader["ProductId"];
                                    string imageUrl = reader["ImageURL"].ToString();

                                    // İlgili ürünü bul ve resim ekle
                                    ProductModel product = products.FirstOrDefault(p => p.ProductId == productId);
                                    if (product != null && !product.Images.Contains(imageUrl)) // Check if image already exists
                                    {
                                        product.Images.Add(imageUrl);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading products: " + ex.Message);
            }

            return products;

        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return latest 4 product. */
        public List<ProductModel> GetNewestProducts(int limit = 4)
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    string query = @"
                        SELECT TOP (@Limit) *
                        FROM Products 
                        ORDER BY CreatedAt DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Limit", limit);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductModel product = new ProductModel
                                {
                                    ProductId = (int)reader["ProductId"],
                                    CompanyId = (int)reader["CompanyId"],
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = (decimal)reader["Price"],
                                    Stock = (int)reader["Stock"],
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    Category = reader["Category"].ToString(),
                                    Rating = Convert.ToSingle(reader["Rating"]),  // Float tipi için Convert.ToSingle kullanın
                                    Favorite = (int)reader["Favorite"],
                                    isAvailable = reader["isAvailable"].ToString(),
                                    Images = new List<string>()
                                };
                                products.Add(product);
                            }
                        }
                    }
                }

                // İkinci bağlantıyı kullanarak resimleri okuyun
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string imageQuery = "SELECT ProductId, ImageURL FROM ProductImages";
                    using (SqlCommand cmd = new SqlCommand(imageQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) // Check if there are rows returned by the query
                            {
                                while (reader.Read())
                                {
                                    int productId = (int)reader["ProductId"];
                                    string imageUrl = reader["ImageURL"].ToString();

                                    // İlgili ürünü bul ve resim ekle
                                    ProductModel product = products.FirstOrDefault(p => p.ProductId == productId);
                                    if (product != null && !product.Images.Contains(imageUrl)) // Check if image already exists
                                    {
                                        product.Images.Add(imageUrl);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading products: " + ex.Message);
            }

            return products;

        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return comments randomly. */
        public List<ProductModel> GetRandomComments(int limit = 4)
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    string query = @"
                        SELECT *
                        FROM Products ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Limit", limit);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductModel product = new ProductModel
                                {
                                    ProductId = (int)reader["ProductId"],
                                    CompanyId = (int)reader["CompanyId"],
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = (decimal)reader["Price"],
                                    Stock = (int)reader["Stock"],
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    Category = reader["Category"].ToString(),
                                    Rating = Convert.ToSingle(reader["Rating"]),  // Float tipi için Convert.ToSingle kullanın
                                    Favorite = (int)reader["Favorite"],
                                    isAvailable = reader["isAvailable"].ToString(),
                                    Images = new List<string>()
                                };
                                products.Add(product);
                            }
                        }
                    }
                }

                // İkinci bağlantıyı kullanarak resimleri okuyun
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string imageQuery = "SELECT ProductId, ImageURL FROM ProductImages";
                    using (SqlCommand cmd = new SqlCommand(imageQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) // Check if there are rows returned by the query
                            {
                                while (reader.Read())
                                {
                                    int productId = (int)reader["ProductId"];
                                    string imageUrl = reader["ImageURL"].ToString();

                                    // İlgili ürünü bul ve resim ekle
                                    ProductModel product = products.FirstOrDefault(p => p.ProductId == productId);
                                    if (product != null && !product.Images.Contains(imageUrl)) // Check if image already exists
                                    {
                                        product.Images.Add(imageUrl);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading products: " + ex.Message);
            }

            return products;

        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /*Return selected product's details */

        public ModelForUserPages ProductDetail(int productId)
        {
            ModelForUserPages modelForUserPages = new ModelForUserPages();
            ProductDetailsModel productDetailsModel = modelForUserPages.productDetailsModel;


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Update clicked value of product 
                    string query = "UPDATE Products SET Clicked = Clicked + 1 WHERE ProductId = @ProductId";

                    using (var command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.ExecuteNonQuery();
                    }



                    query = @"
                        SELECT *
                        FROM Products where ProductId = @productId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {


                            if (reader.Read())
                            {
                                productDetailsModel.Product = new ProductModel();
                                productDetailsModel.Product.ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"));
                                productDetailsModel.Product.CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId"));
                                productDetailsModel.Product.Name = reader.GetString(reader.GetOrdinal("Name"));
                                productDetailsModel.Product.Description = reader.GetString(reader.GetOrdinal("Description"));
                                productDetailsModel.Product.Price = reader.GetDecimal(reader.GetOrdinal("Price"));
                                productDetailsModel.Product.Stock = reader.GetInt32(reader.GetOrdinal("Stock"));
                                productDetailsModel.Product.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                                productDetailsModel.Product.Category = reader.GetString(reader.GetOrdinal("Category"));
                                productDetailsModel.Product.Rating = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("Rating")));
                                productDetailsModel.Product.Favorite = reader.GetInt32(reader.GetOrdinal("Favorite"));
                                productDetailsModel.Product.Clicked = reader.GetInt32(reader.GetOrdinal("Clicked"));
                                productDetailsModel.Product.isAvailable = reader.GetString(reader.GetOrdinal("isAvailable"));

                            }
                        }
                    }


                    // Then, retrieve company details based on CompanyId
                    string companyQuery = @"
                    SELECT *
                    FROM Companies
                    WHERE Id = @companyId";

                    using (SqlCommand cmd = new SqlCommand(companyQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@companyId", productDetailsModel.Product.CompanyId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productDetailsModel.Company = new CompanyModel
                                {
                                    CompanyId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    isAccountActivated = reader.GetBoolean(reader.GetOrdinal("isAccountActivated")),
                                    LogoUrl = reader.IsDBNull(reader.GetOrdinal("LogoUrl")) ? null : reader.GetString(reader.GetOrdinal("LogoUrl")),
                                    BannerUrl = reader.IsDBNull(reader.GetOrdinal("BannerUrl")) ? null : reader.GetString(reader.GetOrdinal("BannerUrl")),
                                    TaxIDNumber = reader.GetString(reader.GetOrdinal("TaxIDNumber")),
                                    IBAN = reader.GetString(reader.GetOrdinal("IBAN")),
                                    isHighlighed = reader.IsDBNull(reader.GetOrdinal("IsHighlighted")) ? null : reader.GetString(reader.GetOrdinal("IsHighlighted")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    Rating = reader.GetInt32(reader.GetOrdinal("Rating"))

                                };
                            }
                        }
                    }



                    string userQuery = @"
                    SELECT *
                    FROM Users
                    WHERE Id = @userId";

                    using (SqlCommand cmd = new SqlCommand(userQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", productDetailsModel.Company.UserId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productDetailsModel.User = new UserModel
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Age = reader.GetInt32(reader.GetOrdinal("Age")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Surname = reader.GetString(reader.GetOrdinal("Surname")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Role = reader.GetString(reader.GetOrdinal("Role")),
                                    Birthdate = reader.GetDateTime(reader.GetOrdinal("Birthdate")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                                };
                            }
                        }
                    }


                    // Ürün yorumlarını al
                    string reviewsQuery = @"
                    SELECT *
                    FROM Reviews 
                    WHERE ProductId = @productId";


                    using (SqlCommand cmd = new SqlCommand(reviewsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            productDetailsModel.ProductReviews = new List<ProductReviewModel>();

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    productDetailsModel.ProductReviews.Add(new ProductReviewModel
                                    {
                                        ReviewId = reader.GetInt32(reader.GetOrdinal("Id")),
                                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId")),
                                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                        Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                                        Review = reader.GetString(reader.GetOrdinal("Review")),
                                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                                    });

                                    //Console.WriteLine($"Review Added");
                                }
                            }

                        }
                    }



                    // Yorum yapan kullanıcıların bilgilerini oku 
                    for (int i = 0; i < productDetailsModel.ProductReviews.Count; i++)
                    {
                        string reviewsUsersQuery = @"
                        SELECT *
                        FROM Users 
                        WHERE Id = @userId";

                        using (SqlCommand cmd = new SqlCommand(reviewsUsersQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@userId", productDetailsModel.ProductReviews[i].UserId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        productDetailsModel.ReviewedUsers.Add(new UserModel
                                        {
                                            UserId = reader.GetInt32(reader.GetOrdinal("Id")),
                                            Age = reader.GetInt32(reader.GetOrdinal("Age")),
                                            Name = reader.GetString(reader.GetOrdinal("Name")),
                                            Surname = reader.GetString(reader.GetOrdinal("Surname")),
                                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                            Email = reader.GetString(reader.GetOrdinal("Email")),
                                            Address = reader.GetString(reader.GetOrdinal("Address")),
                                            Role = reader.GetString(reader.GetOrdinal("Role")),
                                            Birthdate = reader.GetDateTime(reader.GetOrdinal("Birthdate")),
                                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                                        });
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No user found with Id: " + productDetailsModel.ProductReviews[i].UserId);
                                }
                            }
                        }
                    }



                    // ürünün resimlerini döndür 
                    string imagesQuery = @"
                    SELECT *
                    FROM ProductImages
                    WHERE ProductId = @productId";

                    using (SqlCommand cmd = new SqlCommand(imagesQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            productDetailsModel.ProductImages = new List<string>();

                            while (reader.Read())
                            {
                                int imageUrlIndex = reader.GetOrdinal("ImageURL");
                                string imageUrl = reader.GetString(imageUrlIndex);

                                productDetailsModel.ProductImages.Add(imageUrl);
                            }
                        }
                    }





                    query = @"
                        SELECT *
                        FROM Products
                        WHERE Category = @category
                        AND ProductId != @excludedProductId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@category", productDetailsModel.Product.Category);
                        cmd.Parameters.AddWithValue("@excludedProductId", productDetailsModel.Product.ProductId);


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                modelForUserPages.recommendatitons.Add(new ProductModel
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    Category = reader.GetString(reader.GetOrdinal("Category")),
                                    Rating = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("Rating"))),
                                    Favorite = reader.GetInt32(reader.GetOrdinal("Favorite")),
                                    Clicked = reader.GetInt32(reader.GetOrdinal("Clicked")),
                                    isAvailable = reader.GetString(reader.GetOrdinal("isAvailable"))
                                });
                            }
                        }
                    }



                    for (int i = 0; i < modelForUserPages.recommendatitons.Count; i++)
                    {
                        // ürünün resimlerini döndür 
                        imagesQuery = @"
                            SELECT *
                            FROM ProductImages
                            WHERE ProductId = @productId";

                        using (SqlCommand cmd = new SqlCommand(imagesQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@productId", modelForUserPages.recommendatitons[i].ProductId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                productDetailsModel.ProductImages = new List<string>();

                                while (reader.Read())
                                {
                                    int imageUrlIndex = reader.GetOrdinal("ImageURL");
                                    string imageUrl = reader.GetString(imageUrlIndex);

                                    modelForUserPages.recommendatitons[i].Images.Add(imageUrl);
                                }
                            }
                        }
                    }




                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading products: " + ex.Message);
            }
            return modelForUserPages;
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        // Add product to user's cart

        public async Task AddToCart(int? userId, int productId, int companyId, int quantity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    var queryCheck = "SELECT Count FROM ProductsInBasket WHERE UserId = @UserId AND ProductId = @ProductId AND CompanyId = @CompanyId";
                    var queryUpdate = "UPDATE ProductsInBasket SET Count = Count + @Quantity WHERE UserId = @UserId AND ProductId = @ProductId AND CompanyId = @CompanyId";
                    var queryInsert = "INSERT INTO ProductsInBasket (UserId, ProductId, CompanyId, Count) VALUES (@UserId, @ProductId, @CompanyId, @Quantity)";

                    try
                    {
                        // Check if the product already exists in the basket
                        using (SqlCommand command = new SqlCommand(queryCheck, conn))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@ProductId", productId);
                            command.Parameters.AddWithValue("@CompanyId", companyId);

                            var result = await command.ExecuteScalarAsync();

                            if (result != null && result != DBNull.Value) // Product exists
                            {
                                // Update the Count value
                                using (SqlCommand updateCommand = new SqlCommand(queryUpdate, conn))
                                {
                                    updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                                    updateCommand.Parameters.AddWithValue("@UserId", userId);
                                    updateCommand.Parameters.AddWithValue("@ProductId", productId);
                                    updateCommand.Parameters.AddWithValue("@CompanyId", companyId);
                                    await updateCommand.ExecuteNonQueryAsync();
                                }
                            }
                            else // Product does not exist
                            {
                                // Insert a new record
                                using (SqlCommand insertCommand = new SqlCommand(queryInsert, conn))
                                {
                                    insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                                    insertCommand.Parameters.AddWithValue("@UserId", userId);
                                    insertCommand.Parameters.AddWithValue("@ProductId", productId);
                                    insertCommand.Parameters.AddWithValue("@CompanyId", companyId);
                                    await insertCommand.ExecuteNonQueryAsync();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving to cart: " + ex.Message);
            }
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return number of basket products for navbar cart icon. */
        public int GetBasketCount(int? userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SUM(Count) FROM ProductsInBasket WHERE UserId = @UserId";

                using (var command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    object result = command.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                }
            }
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return basket products of the user. */
        public ModelForUserPages GetBasket(int? userId)
        {
            ModelForUserPages modelForUserPages = new ModelForUserPages();
            ProductsInBasket basketProducts = new ProductsInBasket();


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string queryProductIds = @"
                SELECT *
                FROM ProductsInBasket
                WHERE UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(queryProductIds, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            basketProducts.Products.Add(new ProductInBasket
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                CompanyId = reader.GetInt32(2),
                                ProductId = reader.GetInt32(3),
                                count = reader.GetInt32(4)
                            });
                        }
                    }
                }


                for (int i = 0; i < basketProducts.Products.Count; i++)
                {
                    string queryProducts = @"
                    SELECT *
                    FROM Products
                    WHERE ProductId = @ProductId";


                    using (SqlCommand cmd = new SqlCommand(queryProducts, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", basketProducts.Products[i].ProductId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductModel product = new ProductModel
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    Category = reader.GetString(reader.GetOrdinal("Category")),
                                    Rating = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("Rating"))),
                                    Favorite = reader.GetInt32(reader.GetOrdinal("Favorite")),
                                    Clicked = reader.GetInt32(reader.GetOrdinal("Clicked")),
                                    isAvailable = reader.GetString(reader.GetOrdinal("isAvailable"))
                                };
                                basketProducts.Products[i].Product = product;


                            }
                        }
                    }
                }

                // Get images
                for (int i = 0; i < basketProducts.Products.Count; i++)
                {
                    string query = @"
                    SELECT ProductId, ImageUrl
                    FROM ProductImages
                    WHERE ProductId = @productId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", basketProducts.Products[i].ProductId);


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int productId = reader.GetInt32(0);
                                string imageUrl = reader.GetString(1);

                                // Add to the list as a Tuple
                                basketProducts.Products[i].ProductId = productId;
                                basketProducts.Products[i].Images.Add(imageUrl);

                            }
                        }
                    }
                }



                // Get Credit Cards
                string creditCardQuery = @"
                SELECT *
                FROM CreditCards
                WHERE UserId = @userId";

                using (SqlCommand cmd = new SqlCommand(creditCardQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Assuming user.creditCards is a collection to hold the credit card details
                        modelForUserPages.creditCards = new List<CardModel>();

                        while (reader.Read())
                        {
                            modelForUserPages.creditCards.Add(new CardModel
                            {
                                CardId = reader.GetInt32(reader.GetOrdinal("Id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
                                CardHolderName = reader.GetString(reader.GetOrdinal("CardHolderName")),
                                ExpirationDate = reader.GetString(reader.GetOrdinal("ExpirationDate")),
                                CVV = reader.GetString(reader.GetOrdinal("CVV"))
                            });
                        }
                    }
                }

            }

            modelForUserPages.productsInBasket = basketProducts;

            return modelForUserPages;
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return previously deleted basket products of the user. */
        public ProductsInBasket GetPrevDeletedBasket(int? userId)
        {
            ProductsInBasket basketProducts = new ProductsInBasket();


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                SELECT TOP 10 *
                FROM ProductsDeletedFromBasket
                WHERE UserId = @UserId
                ORDER BY DeletedAt DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            basketProducts.Products.Add(new ProductInBasket
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                CompanyId = reader.GetInt32(2),
                                ProductId = reader.GetInt32(3),
                                DeletedAt = reader.GetDateTime(4)
                            });
                        }
                    }
                }




                for (int i = 0; i < basketProducts.Products.Count; i++)
                {
                    string queryProducts = @"
                    SELECT *
                    FROM Products
                    WHERE ProductId = @ProductId";


                    using (SqlCommand cmd = new SqlCommand(queryProducts, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", basketProducts.Products[i].ProductId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductModel product = new ProductModel
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    Category = reader.GetString(reader.GetOrdinal("Category")),
                                    Rating = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("Rating"))),
                                    Favorite = reader.GetInt32(reader.GetOrdinal("Favorite")),
                                    Clicked = reader.GetInt32(reader.GetOrdinal("Clicked")),
                                    isAvailable = reader.GetString(reader.GetOrdinal("isAvailable"))
                                };
                                basketProducts.Products[i].Product = product;


                            }
                        }
                    }
                }

                // Get images
                for (int i = 0; i < basketProducts.Products.Count; i++)
                {
                    query = @"
                    SELECT ProductId, ImageUrl
                    FROM ProductImages
                    WHERE ProductId = @productId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", basketProducts.Products[i].ProductId);


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int productId = reader.GetInt32(0);
                                string imageUrl = reader.GetString(1);

                                // Add to the list as a Tuple
                                basketProducts.Products[i].ProductId = productId;
                                basketProducts.Products[i].Images.Add(imageUrl);

                            }
                        }
                    }
                }

            }
            return basketProducts;
        }





        /* ------------------------------------------------------------------------------------------------------------- */
        /* Delete product from the user's basket. */
        public void DeleteItemInBasket(int? userId, int itemId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();


                /* productId Id al */
                int productId = 0;

                string queryProductIds = @"
                        SELECT ProductId
                        FROM ProductsInBasket
                        WHERE Id = @itemId";

                using (SqlCommand cmd = new SqlCommand(queryProductIds, conn))
                {
                    cmd.Parameters.AddWithValue("@itemId", itemId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            productId = reader.GetInt32(0);
                        }
                    }
                }

                /* Company Id al */
                int companyId = 0;

                queryProductIds = @"
                    SELECT CompanyId
                    FROM Products
                    WHERE ProductId = @productId";

                using (SqlCommand cmd = new SqlCommand(queryProductIds, conn))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            companyId = reader.GetInt32(0);
                        }
                    }
                }




                // Begin transaction
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Check if the ProductId already exists in ProductsDeletedFromBasket
                        string checkQuery = @"
                        SELECT COUNT(*)
                        FROM ProductsDeletedFromBasket
                        WHERE UserId = @UserId AND ProductId = @ProductId";

                        using (SqlCommand checkCommand = new SqlCommand(checkQuery, conn, transaction))
                        {
                            checkCommand.Parameters.AddWithValue("@UserId", userId);
                            checkCommand.Parameters.AddWithValue("@ProductId", productId);

                            int count = (int)checkCommand.ExecuteScalar();

                            if (count == 0)
                            {
                                // ProductId does not exist, proceed with insertion
                                string insertQuery = @"
                                    INSERT INTO ProductsDeletedFromBasket (UserId, CompanyId, ProductId)
                                    VALUES (@UserId, @CompanyId, @ProductId)";

                                using (SqlCommand insertCommand = new SqlCommand(insertQuery, conn, transaction))
                                {
                                    insertCommand.Parameters.AddWithValue("@UserId", userId);
                                    insertCommand.Parameters.AddWithValue("@CompanyId", companyId);
                                    insertCommand.Parameters.AddWithValue("@ProductId", productId);
                                    insertCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        // Delete from ProductsInBasket
                        string deleteQuery = "DELETE FROM ProductsInBasket WHERE Id = @Id";

                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, conn, transaction))
                        {
                            deleteCommand.Parameters.AddWithValue("@Id", itemId);
                            deleteCommand.ExecuteNonQuery();
                        }

                        // Commit transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction on error
                        transaction.Rollback();
                        throw; // Re-throw the exception to handle it further up the call stack
                    }
                }
            }


        }

        /* -------------------------------------------------------------------------------------------------------------- */
        /* Update quantity */
        public void updateQuantity(int? userId, int productId, int companyId, int quantity)
        {
            var query = "UPDATE ProductsInBasket SET Count = @Quantity WHERE UserId = @UserId AND ProductId = @ProductId AND CompanyId = @CompanyId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@CompanyId", companyId);
                    command.ExecuteNonQuery();
                }
            }
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Get User Profile Info */
        public ModelForUserPages GetUserProfileInfo(int? userId)
        {
            ModelForUserPages user = new ModelForUserPages();


            // Get User Info

            string userQuery = @"
                    SELECT *
                    FROM Users
                    WHERE Id = @userId";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(userQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.userProfile = new UserModel
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("Id")),
                                Age = reader.GetInt32(reader.GetOrdinal("Age")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Surname = reader.GetString(reader.GetOrdinal("Surname")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                Birthdate = reader.GetDateTime(reader.GetOrdinal("Birthdate")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            };
                        }
                    }
                }





                // Get Reviews

                userQuery = @"
                    SELECT *
                    FROM Reviews
                    WHERE UserId = @userId";

                using (SqlCommand cmd = new SqlCommand(userQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            user.productReviews.Add(new ProductReviewModel
                            {
                                ReviewId = reader.GetInt32(reader.GetOrdinal("Id")),
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                                Review = reader.GetString(reader.GetOrdinal("Review")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            });

                        }
                    }
                }



                for (int i = 0; i < user.productReviews.Count; i++) {
                    // Get images

                    string query = @"
                        SELECT p.Category, pi.ImageUrl
                        FROM Products p
                        LEFT JOIN ProductImages pi ON p.ProductId = pi.ProductId
                        WHERE p.ProductId = @productId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", user.productReviews[i].ProductId);


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user.productReviews[i].category = reader.GetString(0);
                                string imageUrl = reader.IsDBNull(1) ? null : reader.GetString(1);
                                if (!string.IsNullOrEmpty(imageUrl))
                                {
                                    user.productReviews[i].Images.Add(imageUrl);
                                }

                            }
                        }
                    }
                }





                // Get Credit Cards

                string creditCardQuery = @"
                SELECT *
                FROM CreditCards
                WHERE UserId = @userId";

                using (SqlCommand cmd = new SqlCommand(creditCardQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Assuming user.creditCards is a collection to hold the credit card details
                        user.creditCards = new List<CardModel>();

                        while (reader.Read())
                        {
                            user.creditCards.Add(new CardModel
                            {
                                CardId = reader.GetInt32(reader.GetOrdinal("Id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
                                CardHolderName = reader.GetString(reader.GetOrdinal("CardHolderName")),
                                ExpirationDate = reader.GetString(reader.GetOrdinal("ExpirationDate")),
                                CVV = reader.GetString(reader.GetOrdinal("CVV"))
                            });
                        }
                    }
                }


                // Query to get Product IDs and purchase dates, sorted by purchase date
                string productsBoughtQuery = @"
                    SELECT ProductId, CreatedAt
                    FROM ProductsBought
                    WHERE UserId = @userId
                    ORDER BY CreatedAt DESC";

                List<(int ProductId, DateTime CreatedAt)> boughtProducts = new List<(int, DateTime)>();

                using (SqlCommand cmd = new SqlCommand(productsBoughtQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            boughtProducts.Add((
                                ProductId: reader.GetInt32(reader.GetOrdinal("ProductId")),
                                CreatedAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            ));
                        }
                    }
                }


                // Prepare the list of Product IDs for the query
                var productIds = boughtProducts.Select(p => p.ProductId).ToList();


                if (productIds.Any())
                {

                    string productDetailsQuery = @"
                    SELECT *
                    FROM Products
                    WHERE ProductId IN (" + string.Join(",", productIds) + ")";

                    using (SqlCommand cmd = new SqlCommand(productDetailsQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            user.productsBought = new List<ProductModel>();

                            while (reader.Read())
                            {
                                user.productsBought.Add(new ProductModel
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    Category = reader.GetString(reader.GetOrdinal("Category")),
                                    Rating = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("Rating"))),
                                    Favorite = reader.GetInt32(reader.GetOrdinal("Favorite")),
                                    Clicked = reader.GetInt32(reader.GetOrdinal("Clicked")),
                                    isAvailable = reader.GetString(reader.GetOrdinal("isAvailable"))
                                });
                            }
                        }
                    }
                }




                for (int i = 0; i < user.productsBought.Count; i++)
                {
                    // Get images

                    string query = @"
                        SELECT p.Category, pi.ImageUrl
                        FROM Products p
                        LEFT JOIN ProductImages pi ON p.ProductId = pi.ProductId
                        WHERE p.ProductId = @productId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", user.productsBought[i].ProductId);


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string imageUrl = reader.GetString(1);
                                user.productsBought[i].Images.Add(imageUrl);

                                user.productsBought[i].categoryyy_ = reader.GetString(0);
                            }
                        }
                    }
                }





                // Query to get followed company IDs and follow dates, sorted by follow date
                string followedCompaniesQuery = @"
                    SELECT CompanyId, CreatedAt
                    FROM FollowedCompanies
                    WHERE UserId = @userId
                    ORDER BY CreatedAt DESC";

                List<(int CompanyId, DateTime CreatedAt)> followedCompanies = new List<(int, DateTime)>();

                using (SqlCommand cmd = new SqlCommand(followedCompaniesQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            followedCompanies.Add((
                                CompanyId: reader.GetInt32(reader.GetOrdinal("CompanyId")),
                                CreatedAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            ));
                        }
                    }
                }

                // Prepare the list of Company IDs for the query
                var companyIds = followedCompanies.Select(c => c.CompanyId).ToList();

                if (companyIds.Any())
                {
                    string companyDetailsQuery = @"
                    SELECT Id, CompanyName, Description, Address, PhoneNumber, Email, LogoUrl, BannerUrl
                    FROM Companies
                    WHERE Id IN (" + string.Join(",", companyIds) + ")";

                    using (SqlCommand cmd = new SqlCommand(companyDetailsQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            user.followedCompanies = new List<CompanyModel>();

                            while (reader.Read())
                            {
                                user.followedCompanies.Add(new CompanyModel
                                {
                                    CompanyId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    isAccountActivated = reader.GetBoolean(reader.GetOrdinal("isAccountActivated")),
                                    LogoUrl = reader.IsDBNull(reader.GetOrdinal("LogoUrl")) ? null : reader.GetString(reader.GetOrdinal("LogoUrl")),
                                    BannerUrl = reader.IsDBNull(reader.GetOrdinal("BannerUrl")) ? null : reader.GetString(reader.GetOrdinal("BannerUrl")),
                                    TaxIDNumber = reader.GetString(reader.GetOrdinal("TaxIDNumber")),
                                    IBAN = reader.GetString(reader.GetOrdinal("IBAN")),
                                    isHighlighed = reader.IsDBNull(reader.GetOrdinal("IsHighlighted")) ? null : reader.GetString(reader.GetOrdinal("IsHighlighted")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    Rating = reader.GetInt32(reader.GetOrdinal("Rating"))
                                });
                            }
                        }
                    }
                }
            }

            return user;
        }



        /* -------------------------------------------------------------------------------------------------------------- */
        /* Add card to the database */
        public void AddCard(int? userId, CardModel card)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    INSERT INTO CreditCards (UserId, CardNumber, CardHolderName, ExpirationDate, CVV)
                    VALUES (@UserId, @CardNumber, @CardHolderName, @ExpirationDate, @CVV)";


                // Create and configure the SqlCommand
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@CardNumber", card.CardNumber);
                    cmd.Parameters.AddWithValue("@CardHolderName", card.CardHolderName);
                    cmd.Parameters.AddWithValue("@ExpirationDate", card.ExpirationDate);
                    cmd.Parameters.AddWithValue("@CVV", card.CVV);

                    // Execute the command
                    cmd.ExecuteNonQuery();
                }
            }
        }



        /* ------------------------------------------------------------------------------------------------------------- */
        /* Delete product from the user's basket. */
        public void ClearCart(int? userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Begin transaction
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Get all products in the basket for the user
                        string queryProductsInBasket = @"
                    SELECT pib.ProductId, p.CompanyId
                    FROM ProductsInBasket pib
                    JOIN Products p ON pib.ProductId = p.ProductId
                    WHERE pib.UserId = @UserId";

                        var productsInBasket = new List<(int ProductId, int CompanyId)>();

                        using (SqlCommand cmd = new SqlCommand(queryProductsInBasket, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserId", userId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    productsInBasket.Add((reader.GetInt32(0), reader.GetInt32(1)));
                                }
                            }
                        }

                        // Insert products into ProductsBought if they do not already exist
                        string checkQuery = @"
                    SELECT COUNT(*)
                    FROM ProductsBought
                    WHERE UserId = @UserId AND ProductId = @ProductId";

                        string insertQuery = @"
                    INSERT INTO ProductsBought (UserId, CompanyId, ProductId)
                    VALUES (@UserId, @CompanyId, @ProductId)";

                        foreach (var product in productsInBasket)
                        {
                            using (SqlCommand checkCommand = new SqlCommand(checkQuery, conn, transaction))
                            {
                                checkCommand.Parameters.AddWithValue("@UserId", userId);
                                checkCommand.Parameters.AddWithValue("@ProductId", product.ProductId);

                                int count = (int)checkCommand.ExecuteScalar();

                                if (count == 0)
                                {
                                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, conn, transaction))
                                    {
                                        insertCommand.Parameters.AddWithValue("@UserId", userId);
                                        insertCommand.Parameters.AddWithValue("@CompanyId", product.CompanyId);
                                        insertCommand.Parameters.AddWithValue("@ProductId", product.ProductId);
                                        insertCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        // Delete all products from ProductsInBasket for the user
                        string deleteQuery = "DELETE FROM ProductsInBasket WHERE UserId = @UserId";

                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, conn, transaction))
                        {
                            deleteCommand.Parameters.AddWithValue("@UserId", userId);
                            deleteCommand.ExecuteNonQuery();
                        }

                        // Commit transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction on error
                        transaction.Rollback();
                        throw; // Re-throw the exception to handle it further up the call stack
                    }
                }
            }

        }



        public void AddReview(ProductReviewModel model, int? userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    INSERT INTO Reviews (ProductId, CompanyId, UserId, Rating, Review, CreatedAt)
                    VALUES (@ProductId, @CompanyId, @UserId, @Rating, @Review, @CreatedAt)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@ProductId", model.ProductId);
                    cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Rating", model.Rating);
                    cmd.Parameters.AddWithValue("@Review", model.Review);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }



                // Ortalama puanı hesaplayan SQL komutu
                query = @"
                UPDATE Products
                SET Rating = (
                    SELECT AVG(CAST(Rating AS FLOAT))
                    FROM Reviews
                    WHERE ProductId = @ProductId
                )
                WHERE ProductId = @ProductId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", model.ProductId);

                    // SQL komutunu çalıştır
                    cmd.ExecuteNonQuery();
                }
            }
        }




        /* ------------------------------------------------------------------------------------------------------------- */
        /* Return all company info. */
        public List<CompanyModel> GetAllCompanies()
        {
            List<CompanyModel> companies = new List<CompanyModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();


                try
                {
                    // Then, retrieve company details based on CompanyId
                    string companyQuery = @"
                    SELECT *
                    FROM Companies
                   ";

                    using (SqlCommand cmd = new SqlCommand(companyQuery, conn))
                    {

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                companies.Add(new CompanyModel
                                {
                                    CompanyId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    isAccountActivated = reader.GetBoolean(reader.GetOrdinal("isAccountActivated")),
                                    LogoUrl = reader.IsDBNull(reader.GetOrdinal("LogoUrl")) ? null : reader.GetString(reader.GetOrdinal("LogoUrl")),
                                    BannerUrl = reader.IsDBNull(reader.GetOrdinal("BannerUrl")) ? null : reader.GetString(reader.GetOrdinal("BannerUrl")),
                                    TaxIDNumber = reader.GetString(reader.GetOrdinal("TaxIDNumber")),
                                    IBAN = reader.GetString(reader.GetOrdinal("IBAN")),
                                    isHighlighed = reader.IsDBNull(reader.GetOrdinal("IsHighlighted")) ? null : reader.GetString(reader.GetOrdinal("IsHighlighted")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    Rating = reader.GetInt32(reader.GetOrdinal("Rating"))

                                });

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw; // Re-throw the exception to handle it further up the call stack
                }
            }
            return companies;
        }



        /* ------------------------------------------------------------------------------------------------------------- */
        /* Return products by category */
        public List<ProductModel> GetProductsByCategory(string category)
        {
            List<ProductModel> products = new List<ProductModel>();



            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Ürünleri kategoriye göre filtreleyen sorgu
                    string productQuery = "SELECT * FROM Products WHERE Category = @Category";
                    using (SqlCommand cmd = new SqlCommand(productQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Category", category);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductModel product = new ProductModel
                                {
                                    ProductId = (int)reader["ProductId"],
                                    CompanyId = (int)reader["CompanyId"],
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = (decimal)reader["Price"],
                                    Stock = (int)reader["Stock"],
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    Category = reader["Category"].ToString(),
                                    Rating = Convert.ToSingle(reader["Rating"]),  // Float tipi için Convert.ToSingle
                                    Favorite = (int)reader["Favorite"],
                                    isAvailable = reader["isAvailable"].ToString(),
                                    Images = new List<string>()
                                };
                                products.Add(product);

                            }
                        }
                    }
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string imageQuery = "SELECT ProductId, ImageURL FROM ProductImages";
                    using (SqlCommand cmd = new SqlCommand(imageQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) // Sorgudan dönen satır olup olmadığını kontrol et
                            {
                                while (reader.Read())
                                {
                                    int productId = (int)reader["ProductId"];
                                    string imageUrl = reader["ImageURL"].ToString();

                                    // İlgili ürünü bul ve resim ekle
                                    ProductModel product = products.FirstOrDefault(p => p.ProductId == productId);
                                    if (product != null && !product.Images.Contains(imageUrl)) // Resim zaten var mı kontrol et
                                    {
                                        product.Images.Add(imageUrl);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ürünler okunurken hata oluştu: " + ex.Message);
            }

            return products;
        }


        /* ------------------------------------------------------------------------------------------------------------- */
        /* Eğer main category seçilmiş ise tüm alt categorilerdeki ürünleri döndürmek için alt kategorileri bul. */
        public List<ProductModel> GetSubcategoriesByMainCategory(string mainCategory)
        {
            List<ProductModel> products = new List<ProductModel>();
            List<string> subcategories = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string subcategoryQuery = "SELECT CategoryName FROM Categories WHERE MainCategory = @MainCategory";
                    using (SqlCommand cmd = new SqlCommand(subcategoryQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MainCategory", mainCategory);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                subcategories.Add(reader["CategoryName"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subkategoriler alınırken hata oluştu: " + ex.Message);
            }




            foreach (var subcategory in subcategories)
            {
                var subcategoryProducts = GetProductsByCategory(subcategory);
                products.AddRange(subcategoryProducts);
            }

            return products;
        }



        /* ------------------------------------------------------------------------------------------------------------- */
        /* Eğer main category seçilmiş ise tüm alt categorilerdeki ürünleri döndürmek için alt kategorileri bul. */
        public ModelForUserPages CompanyDetails(int companyId)
        {
            ModelForUserPages modelForUserPages = new ModelForUserPages();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    string companyQuery = @"
                    SELECT *
                    FROM Companies
                    WHERE Id = @CompanyId
                   ";

                    using (SqlCommand cmd = new SqlCommand(companyQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                modelForUserPages.companyDetails = new CompanyModel
                                {
                                    CompanyId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    isAccountActivated = reader.GetBoolean(reader.GetOrdinal("isAccountActivated")),
                                    LogoUrl = reader.IsDBNull(reader.GetOrdinal("LogoUrl")) ? null : reader.GetString(reader.GetOrdinal("LogoUrl")),
                                    BannerUrl = reader.IsDBNull(reader.GetOrdinal("BannerUrl")) ? null : reader.GetString(reader.GetOrdinal("BannerUrl")),
                                    TaxIDNumber = reader.GetString(reader.GetOrdinal("TaxIDNumber")),
                                    IBAN = reader.GetString(reader.GetOrdinal("IBAN")),
                                    isHighlighed = reader.IsDBNull(reader.GetOrdinal("IsHighlighted")) ? null : reader.GetString(reader.GetOrdinal("IsHighlighted")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    Rating = reader.GetInt32(reader.GetOrdinal("Rating"))

                                };

                            }
                        }
                    }


                    string productQuery = "SELECT * FROM Products WHERE CompanyId = @CompanyId";
                    using (SqlCommand cmd = new SqlCommand(productQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductModel product = new ProductModel
                                {
                                    ProductId = (int)reader["ProductId"],
                                    CompanyId = (int)reader["CompanyId"],
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = (decimal)reader["Price"],
                                    Stock = (int)reader["Stock"],
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    Category = reader["Category"].ToString(),
                                    Rating = Convert.ToSingle(reader["Rating"]),  // Float tipi için Convert.ToSingle
                                    Favorite = (int)reader["Favorite"],
                                    isAvailable = reader["isAvailable"].ToString(),
                                    Images = new List<string>()
                                };
                                modelForUserPages.companyProducts.Add(product);

                            }
                        }
                    }





                    for (int i = 0; i < modelForUserPages.companyProducts.Count; i++)
                    {
                        // Get images and category
                        string query = @"
                        SELECT p.Category, pi.ImageUrl
                        FROM Products p
                        LEFT JOIN ProductImages pi ON p.ProductId = pi.ProductId
                        WHERE p.ProductId = @productId";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@productId", modelForUserPages.companyProducts[i].ProductId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string imageUrl = reader.IsDBNull(1) ? null : reader.GetString(1);
                                    if (!string.IsNullOrEmpty(imageUrl))
                                    {
                                        modelForUserPages.companyProducts[i].Images.Add(imageUrl);
                                    }

                                    modelForUserPages.companyProducts[i].Category = reader.GetString(0);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Company details alınırken hata oluştu: " + ex.Message);
            }


            return modelForUserPages;
        }





        /* ------------------------------------------------------------------------------------------------------------- */
        /* Satın alınan ürünlerin quantity düşür ve sold arttır. */
        public void PurchaseBasket(int[] productIds, int[] productQuantities)
        {
            if (productIds.Length != productQuantities.Length)
            {
                throw new ArgumentException("Product IDs and quantities arrays must have the same length.");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                   
                    try
                    {
                        for (int i = 0; i < productIds.Length; i++)
                        {
                            int productId = productIds[i];
                            int quantity = productQuantities[i];

                             

                            // Update Stock and Sold values in the Products table
                            string updateQuery = @"
                                UPDATE Products
                                SET Stock = Stock - @Quantity,
                                    Sold = Sold + @Quantity
                                WHERE ProductId = @ProductId
                                AND Stock >= @Quantity"; // Ensures stock is not negative

                            using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@ProductId", productId);
                                cmd.Parameters.AddWithValue("@Quantity", quantity);

                                int rowsAffected = cmd.ExecuteNonQuery();

                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Basket purchase error: " + ex.Message);
            }
        }







    }
}
