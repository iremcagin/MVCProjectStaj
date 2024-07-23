﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Diagnostics;

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
                    return (int)command.ExecuteScalar();
                }
            }
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return basket products of the user. */
        public ProductsInBasket GetBasket(int? userId)
        {
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
                                // SQL komutlarını hazırlayın
                                string deleteQuery = "DELETE FROM ProductsInBasket WHERE Id = @Id";
                                string insertQuery = "INSERT INTO ProductsDeletedFromBasket (UserId, CompanyId, ProductId) VALUES(@UserId, @CompanyId, @ProductId)";

                                // Ürünü silinenler tablosuna ekleme
                                using (SqlCommand insertCommand = new SqlCommand(insertQuery, conn, transaction))
                                {
                                    insertCommand.Parameters.AddWithValue("@UserId", userId);
                                    insertCommand.Parameters.AddWithValue("@CompanyId", companyId);
                                    insertCommand.Parameters.AddWithValue("@ProductId", productId);
                                    insertCommand.ExecuteNonQuery();
                                }

                                // Ürünü sepetten silme
                                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, conn, transaction))
                                {
                                    deleteCommand.Parameters.AddWithValue("@Id", itemId);
                                    deleteCommand.ExecuteNonQuery();
                                }

                               

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw;
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



    }
}
