using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using static NuGet.Packaging.PackagingConstants;

namespace myProject.Models
{
    public class _SellerDatabaseControlModel
    {

        private string _connectionString;

        public void Dispose(string _connectionString)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Dispose();
        }

        public _SellerDatabaseControlModel(string connectionString)
        {
            _connectionString = connectionString;

        }

        /* --------------------------------------------------- Dashboard Page --------------------------------------------------- */

        public ModelForSellerPages Dashboard(int? userId)
        {
            ModelForSellerPages modelForSellerPages = new ModelForSellerPages();

           using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                int companyId = GetCompanyId(userId);

                // Toplam ürün sayısı
                string query = "SELECT COUNT(*) FROM Products WHERE CompanyId=@CompanyId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    modelForSellerPages.TotalProducts = (int)cmd.ExecuteScalar();
                }

                // Toplam yorum sayısı
                query = "SELECT COUNT(*) FROM Reviews WHERE CompanyId=@CompanyId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    modelForSellerPages.TotalReviews = (int)cmd.ExecuteScalar();
                }

                // Toplam müşteri sayısı
                query = "SELECT COUNT(DISTINCT UserId) FROM ProductsBought WHERE CompanyId=@CompanyId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    modelForSellerPages.TotalUsers = (int)cmd.ExecuteScalar();
                }


                // Ortalama değerlendirme
                query = @"
                SELECT AVG(r.Rating) AS AverageRating
                FROM Reviews r
                WHERE r.CompanyId = @CompanyId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    modelForSellerPages.AverageRating = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                // En çok yorum alan ürün
                query = @"
                SELECT TOP 1 p.*, COUNT(r.Id) AS ReviewCount
                FROM Products p
                JOIN Reviews r ON p.ProductId = r.ProductId
                WHERE p.CompanyId = @CompanyId
                GROUP BY p.ProductId, p.CompanyId, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt, p.Category, p.Rating, p.Favorite, p.Clicked, p.isAvailable, p.Sold
                ORDER BY ReviewCount DESC";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelForSellerPages.mostReviewedProduct = new ProductModel
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
                                isAvailable = reader.GetString(reader.GetOrdinal("isAvailable")),
                                Sold = reader.GetInt32(reader.GetOrdinal("Sold"))
                            };
                        }
                    }
                }

                // En çok yorum alan ürünün resimleri
                if (modelForSellerPages.mostReviewedProduct != null)
                {
                    string imageQuery = "SELECT ImageURL FROM ProductImages WHERE ProductId = @ProductId";
                    using (SqlCommand cmd = new SqlCommand(imageQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", modelForSellerPages.mostReviewedProduct.ProductId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (modelForSellerPages.mostReviewedProduct.Images == null)
                            {
                                modelForSellerPages.mostReviewedProduct.Images = new List<string>();
                            }
                            while (reader.Read())
                            {
                                string imageUrl = reader["ImageURL"].ToString();
                                if (!string.IsNullOrWhiteSpace(imageUrl))
                                {
                                    modelForSellerPages.mostReviewedProduct.Images.Add(imageUrl);
                                }
                            }
                        }
                    }
                }


                // En çok görüntülenen
                query = @"
                SELECT TOP 1 p.*
                FROM Products p
                WHERE p.CompanyId = @CompanyId
                ORDER BY p.Clicked DESC";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelForSellerPages.mostClickedProduct = new ProductModel
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
                                isAvailable = reader.GetString(reader.GetOrdinal("isAvailable")),
                                Sold = reader.GetInt32(reader.GetOrdinal("Sold"))
                            };
                        }
                    }
                }

                // En çok beğenilen ürünün resimleri
                if (modelForSellerPages.mostClickedProduct != null)
                {
                    string imageQuery = "SELECT ImageURL FROM ProductImages WHERE ProductId = @ProductId";
                    using (SqlCommand cmd = new SqlCommand(imageQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", modelForSellerPages.mostClickedProduct.ProductId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (modelForSellerPages.mostClickedProduct.Images == null)
                            {
                                modelForSellerPages.mostClickedProduct.Images = new List<string>();
                            }
                            while (reader.Read())
                            {
                                string imageUrl = reader["ImageURL"].ToString();
                                if (!string.IsNullOrWhiteSpace(imageUrl))
                                {
                                    modelForSellerPages.mostClickedProduct.Images.Add(imageUrl);
                                }
                            }
                        }
                    }
                }






                // En çok beğenilen ürün
                query = @"
                SELECT TOP 1 p.*
                FROM Products p
                WHERE p.CompanyId = @CompanyId
                ORDER BY p.Favorite DESC";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelForSellerPages.mostLikedProduct = new ProductModel
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
                                isAvailable = reader.GetString(reader.GetOrdinal("isAvailable")),
                                Sold = reader.GetInt32(reader.GetOrdinal("Sold"))
                            };
                        }
                    }
                }

                // En çok beğenilen ürünün resimleri
                if (modelForSellerPages.mostLikedProduct != null)
                {
                    string imageQuery = "SELECT ImageURL FROM ProductImages WHERE ProductId = @ProductId";
                    using (SqlCommand cmd = new SqlCommand(imageQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", modelForSellerPages.mostLikedProduct.ProductId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (modelForSellerPages.mostLikedProduct.Images == null)
                            {
                                modelForSellerPages.mostLikedProduct.Images = new List<string>();
                            }
                            while (reader.Read())
                            {
                                string imageUrl = reader["ImageURL"].ToString();
                                if (!string.IsNullOrWhiteSpace(imageUrl))
                                {
                                    modelForSellerPages.mostLikedProduct.Images.Add(imageUrl);
                                }
                            }
                        }
                    }
                }

                // En çok satın alınan ürün
                query = @"
                SELECT TOP 1 p.*, COUNT(pb.ProductId) AS PurchaseCount
                FROM Products p
                JOIN ProductsBought pb ON p.ProductId = pb.ProductId
                WHERE p.CompanyId = @CompanyId
                GROUP BY p.ProductId, p.CompanyId, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt, p.Category, p.Rating, p.Favorite, p.Clicked, p.isAvailable, p.Sold
                ORDER BY PurchaseCount DESC";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelForSellerPages.mostPurchasedProduct = new ProductModel
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
                                isAvailable = reader.GetString(reader.GetOrdinal("isAvailable")),
                                Sold = reader.GetInt32(reader.GetOrdinal("Sold"))
                            };
                        }
                    }
                }

                // En çok satın alınan ürünün resimleri
                if (modelForSellerPages.mostPurchasedProduct != null)
                {
                    string imageQuery = "SELECT ImageURL FROM ProductImages WHERE ProductId = @ProductId";
                    using (SqlCommand cmd = new SqlCommand(imageQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", modelForSellerPages.mostPurchasedProduct.ProductId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (modelForSellerPages.mostPurchasedProduct.Images == null)
                            {
                                modelForSellerPages.mostPurchasedProduct.Images = new List<string>();
                            }
                            while (reader.Read())
                            {
                                string imageUrl = reader["ImageURL"].ToString();
                                if (!string.IsNullOrWhiteSpace(imageUrl))
                                {
                                    modelForSellerPages.mostPurchasedProduct.Images.Add(imageUrl);
                                }
                            }
                        }
                    }
                }

                // Günlük satışlar
                query = @"
                SELECT 
                    DAY(pb.CreatedAt) AS Day,
                    MONTH(pb.CreatedAt) AS Month,
                    YEAR(pb.CreatedAt) AS Year,
                    COUNT(pb.ProductId) AS DailySalesCount,
                    SUM(p.Price) AS DailyRevenue
                FROM ProductsBought pb
                INNER JOIN Products p ON pb.ProductId = p.ProductId
                WHERE p.CompanyId = @CompanyId
                GROUP BY 
                    DAY(pb.CreatedAt), MONTH(pb.CreatedAt), YEAR(pb.CreatedAt)
                ORDER BY 
                    YEAR(pb.CreatedAt) DESC, MONTH(pb.CreatedAt) DESC, DAY(pb.CreatedAt) DESC";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        modelForSellerPages.DailySales = new List<DailySalesModel>();
                        while (reader.Read())
                        {
                            DailySalesModel dailySales = new DailySalesModel
                            {
                                Day = reader.GetInt32(reader.GetOrdinal("Day")),
                                Month = reader.GetInt32(reader.GetOrdinal("Month")),
                                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                                SalesCount = reader.GetInt32(reader.GetOrdinal("DailySalesCount")),
                                Revenue = reader.GetDecimal(reader.GetOrdinal("DailyRevenue"))
                            };
                            modelForSellerPages.DailySales.Add(dailySales);
                        }
                    }
                }



                query = @"
                    SELECT 
                        DAY(pb.CreatedAt) AS Day,
                        MONTH(pb.CreatedAt) AS Month,
                        YEAR(pb.CreatedAt) AS Year,
                        SUM(p.Price) AS DailyRevenue
                    FROM 
                        ProductsBought pb
                    INNER JOIN 
                        Products p ON pb.ProductId = p.ProductId
                    WHERE p.CompanyId = @CompanyId
                    GROUP BY 
                        DAY(pb.CreatedAt),
                        MONTH(pb.CreatedAt),
                        YEAR(pb.CreatedAt)
                    ORDER BY 
                        YEAR(pb.CreatedAt) DESC, 
                        MONTH(pb.CreatedAt) DESC, 
                        DAY(pb.CreatedAt) DESC";


                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        modelForSellerPages.DailyRevenues = new List<DailyRevenue>();
                        while (reader.Read())
                        {
                            modelForSellerPages.DailyRevenues.Add(new DailyRevenue
                            {
                                Day = reader.GetInt32(reader.GetOrdinal("Day")),
                                Month = reader.GetInt32(reader.GetOrdinal("Month")),
                                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                                DailyRevenueAmount = reader.GetDecimal(reader.GetOrdinal("DailyRevenue"))
                            });
                        }
                    }
                }




                // Toplam satışlar
                query = @"
                SELECT 
                    COUNT(pb.ProductId) AS TotalSalesCount,
                    SUM(p.Price) AS TotalRevenue
                FROM ProductsBought pb
                INNER JOIN Products p ON pb.ProductId = p.ProductId
                WHERE p.CompanyId = @CompanyId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelForSellerPages.TotalSalesCount = reader.GetInt32(reader.GetOrdinal("TotalSalesCount"));
                            modelForSellerPages.TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue"));
                        }
                    }
                }


                query = @"
                    SELECT 
                        p.Category AS Category,
                        COUNT(pb.ProductId) AS TotalSales
                    FROM 
                        ProductsBought pb
                    INNER JOIN 
                        Products p ON pb.ProductId = p.ProductId
                     WHERE 
                        p.CompanyId = @CompanyId
                    GROUP BY 
                        p.Category
                    ORDER BY 
                        TotalSales DESC";


                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            modelForSellerPages.salesByCategory.Add(new SalesByCategory
                            {
                                Category = reader.GetString(reader.GetOrdinal("Category")),
                                TotalSales = reader.GetInt32(reader.GetOrdinal("TotalSales"))
                            });
                        }
                    }
                }
            }

            return modelForSellerPages;
        }






        /* --------------------------------------------------- Products Page --------------------------------------------------- */
        public void saveProductToDatabase(ProductModel newProduct, int? userId)
        {

            int productId = 0;
            int companyId = GetCompanyId(userId);

           using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection Opened.");

                try
                {
                    string query = @"INSERT INTO Products (CompanyId, Name, Description, Price, Stock, CreatedAt, Category, Rating, Favorite, isAvailable)
                             VALUES (@CompanyId, @Name, @Description, @Price, @Stock, @CreatedAt, @Category, @Rating, @Favorite, @isAvailable);
                             SELECT SCOPE_IDENTITY();"; // SCOPE_IDENTITY ile eklenen son kaydın Id değerini alır

                    SqlCommand cmd = new SqlCommand(query, connection);

                    // Parametreleri ayarla
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@Name", newProduct.Name);
                    cmd.Parameters.AddWithValue("@Description", newProduct.Description);
                    cmd.Parameters.AddWithValue("@Price", newProduct.Price);
                    cmd.Parameters.AddWithValue("@Stock", newProduct.Stock);
                    cmd.Parameters.AddWithValue("@CreatedAt", newProduct.CreatedAt);
                    cmd.Parameters.AddWithValue("@Category", newProduct.Category);
                    cmd.Parameters.AddWithValue("@Rating", newProduct.Rating);
                    cmd.Parameters.AddWithValue("@Favorite", newProduct.Favorite);
                    cmd.Parameters.AddWithValue("@isAvailable", newProduct.isAvailable);


                    productId = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                if (newProduct.Images != null && newProduct.Images.Count > 0)
                {
                    foreach (string imageUrl in newProduct.Images)
                    {

                        try
                        {
                            string query = @"INSERT INTO ProductImages (ProductId, ImageUrl)
                                 VALUES (@ProductId, @ImageUrl)";

                            SqlCommand cmd = new SqlCommand(query, connection);

                            // Parametreleri ayarla
                            cmd.Parameters.AddWithValue("@ProductId", productId);
                            cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);

                            // Sorguyu çalıştır
                            int rowsAffected = cmd.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);

                        }
                    }
                }

            }
        }


        public List<ProductModel> getAllProducts(int? userId)
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
                // İlk bağlantıyı açarak ürünleri okuyun
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();


                    int companyId = GetCompanyId(userId);

                    string productQuery = "SELECT * FROM Products WHERE CompanyId = @CompanyId";
                    using (SqlCommand cmd = new SqlCommand(productQuery, connection))
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
                                    Rating = Convert.ToSingle(reader["Rating"]),  // Float tipi için Convert.ToSingle kullanın
                                    Favorite = (int)reader["Favorite"],
                                    isAvailable = reader["isAvailable"].ToString(),
                                    Sold = reader.GetInt32(reader.GetOrdinal("Sold")),
                                    Images = new List<string>()
                                };
                                products.Add(product);
                            }
                        }
                    }



                    string imageQuery = "SELECT ProductId, ImageURL FROM ProductImages";
                    using (SqlCommand cmd = new SqlCommand(imageQuery, connection))
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


                    for (int i = 0; i < products.Count; i++)
                    {
                        string query = @"
                        SELECT Id, ProductId, CompanyId, UserId, Rating, Review, CreatedAt 
                        FROM Reviews WHERE ProductId = @ProductId
                        ORDER BY CreatedAt DESC";


                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ProductId", products[i].ProductId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Okunan her satırı işleyin
                                while (reader.Read())
                                {
                                    products[i].reviews.Add(new ProductReviewModel
                                    {
                                        ReviewId = (int)reader["Id"],
                                        ProductId = (int)reader["ProductId"],
                                        CompanyId = (int)reader["CompanyId"],
                                        UserId = (int)reader["UserId"],
                                        Rating = (int)reader["Rating"],
                                        Review = reader["Review"].ToString(),
                                        CreatedAt = (DateTime)reader["CreatedAt"]
                                    });
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





        public void RemoveProductImage(int productId, string imageName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM ProductImages WHERE ProductId = @ProductId AND ImageUrl = @ImageUrl";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@ImageUrl", imageName);
                    command.ExecuteNonQuery();
                }
            }
        }



        public void UpdateProduct(ProductModel model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Ürün bilgilerini güncelleme sorgusu
                var updateProductQuery = @"
                UPDATE Products 
                SET Name = @Name, 
                    Description = @Description, 
                    Price = @Price, 
                    Stock = @Stock, 
                    Category = @Category, 
                    isAvailable = @IsAvailable 
                WHERE ProductId = @ProductId";

                using (var command = new SqlCommand(updateProductQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", model.Name);
                    command.Parameters.AddWithValue("@Description", model.Description);
                    command.Parameters.AddWithValue("@Price", model.Price);
                    command.Parameters.AddWithValue("@Stock", model.Stock);
                    command.Parameters.AddWithValue("@Category", model.Category);
                    command.Parameters.AddWithValue("@IsAvailable", model.isAvailable);
                    command.Parameters.AddWithValue("@ProductId", model.ProductId);

                    command.ExecuteNonQuery();
                }

                var insertImageQuery = "INSERT INTO ProductImages (ProductId, ImageUrl) VALUES (@ProductId, @ImageUrl)";
                foreach (var imageUrl in model.Images)
                {
                    using (var insertCommand = new SqlCommand(insertImageQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@ProductId", model.ProductId);
                        insertCommand.Parameters.AddWithValue("@ImageUrl", imageUrl);
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }




        //--------------------------------------------------------------------------------------------------------------
        public int GetCompanyId(int? userId)
        {
            int companyId = 0;

           using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();


                string query = "SELECT Id FROM Companies WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            companyId = (int)reader["Id"];
                        }
                    }
                }
            }

            return companyId;
        }




        //--------------------------------------------------------------------------------------------------------------
        public List<ProductModel> GetAllOrders(int? userId)
        {
            List<ProductModel> orders = new List<ProductModel>();
            List<int> productIds = new List<int>();

            try
            {
                // İlk bağlantıyı açarak ürünleri okuyun
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // get company id
                    int companyId = GetCompanyId(userId);



                    string productQuery = "SELECT * FROM ProductsBought WHERE CompanyId = @CompanyId ORDER BY CreatedAt DESC";
                    using (SqlCommand cmd = new SqlCommand(productQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productIds.Add((int)reader["ProductId"]);
                            }
                        }
                    }


                    for (int i = 0; i < productIds.Count; i++)
                    {
                        string query = "SELECT * FROM Products WHERE ProductId = @ProductId";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ProductId", productIds[i]);

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
                                        Sold = reader.GetInt32(reader.GetOrdinal("Sold")),
                                        Images = new List<string>()
                                    };
                                    orders.Add(product);
                                }
                            }
                        }
                    }



                    for (int i = 0; i < orders.Count; i++)
                    {
                        string imageQuery = "SELECT ImageURL FROM ProductImages where ProductId= @ProductId";
                        using (SqlCommand cmd = new SqlCommand(imageQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@ProductId", orders[i].ProductId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string imageUrl = reader["ImageURL"].ToString();
                                    orders[i].Images.Add(imageUrl);

                                }
                            }
                        }
                    }



                    for (int i = 0; i < orders.Count; i++)
                    {
                        string query = @"
                        SELECT Id, ProductId, CompanyId, UserId, Rating, Review, CreatedAt 
                        FROM Reviews WHERE ProductId = @ProductId
                        ORDER BY CreatedAt DESC";


                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ProductId", orders[i].ProductId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Okunan her satırı işleyin
                                while (reader.Read())
                                {
                                    orders[i].reviews.Add(new ProductReviewModel
                                    {
                                        ReviewId = (int)reader["Id"],
                                        ProductId = (int)reader["ProductId"],
                                        CompanyId = (int)reader["CompanyId"],
                                        UserId = (int)reader["UserId"],
                                        Rating = (int)reader["Rating"],
                                        Review = reader["Review"].ToString(),
                                        CreatedAt = (DateTime)reader["CreatedAt"]
                                    });
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading orders: " + ex.Message);
            }

            return orders;
        }




        //--------------------------------------------------------------------------------------------------------------

        public List<string> getAllCategories()
        {
            List<string> categories = new List<string>();

            try
            {
                // İlk bağlantıyı açarak ürünleri okuyun
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    // Categorileri döndür
                    string query = "SELECT CategoryName FROM Categories";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(
                                    reader["CategoryName"].ToString()
                                );
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading categories: " + ex.Message);
            }

            return categories;
        }





        //--------------------------------------------------------------------------------------------------------------
        public List<UserModel> GetAllCustomers(int? userId)
        {
            List<UserModel> customers = new List<UserModel>();
            List<int> customerIds = new List<int>();

            try
            {
                // İlk bağlantıyı açarak ürünleri okuyun
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // get company id
                    int companyId = GetCompanyId(userId);



                    string productQuery = "SELECT * FROM ProductsBought WHERE CompanyId = @CompanyId ORDER BY CreatedAt DESC";
                    using (SqlCommand cmd = new SqlCommand(productQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customerIds.Add((int)reader["UserId"]);
                            }
                        }
                    }


                    for (int i = 0; i < customerIds.Count; i++)
                    {
                        string query = "SELECT * FROM Users WHERE Id = @customerId";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@customerId", customerIds[i]);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UserModel product = new UserModel
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
                                    customers.Add(product);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading orders: " + ex.Message);
            }

            return customers;
        }



        //--------------------------------------------------------------------------------------------------------------
        public List<UserModel> GetAllFollowers(int? userId)
        {
            List<UserModel> followers = new List<UserModel>();
            List<int> customerIds = new List<int>();

            try
            {
                // İlk bağlantıyı açarak ürünleri okuyun
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // get company id
                    int companyId = GetCompanyId(userId);



                    string productQuery = "SELECT * FROM FollowedCompanies WHERE CompanyId = @CompanyId ORDER BY CreatedAt DESC";
                    using (SqlCommand cmd = new SqlCommand(productQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customerIds.Add((int)reader["UserId"]);
                            }
                        }
                    }


                    for (int i = 0; i < customerIds.Count; i++)
                    {
                        string query = "SELECT * FROM Users WHERE Id = @customerId";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@customerId", customerIds[i]);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UserModel product = new UserModel
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
                                    followers.Add(product);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading orders: " + ex.Message);
            }

            return followers;
        }

        //--------------------------------------------------------------------------------------------------------------
        public ModelForSellerPages GetCompanyInfo(int? userId)
        {
            ModelForSellerPages modelForSellerPages = new ModelForSellerPages();

            try
            {
                // İlk bağlantıyı açarak ürünleri okuyun
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // get company id
                    int companyId = GetCompanyId(userId);

                    string query = "SELECT * FROM Companies WHERE Id = @companyId";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@companyId", companyId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                modelForSellerPages.Company = new CompanyModel
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

                    using (SqlCommand cmd = new SqlCommand(userQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                modelForSellerPages.User = new UserModel
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

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading company info: " + ex.Message);
            }
            return modelForSellerPages;
        }



    }
}
