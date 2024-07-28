using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace myProject.Models
{
    public class _AdminDatabaseControlModel
    {



        private string _connectionString;

        public void Dispose(string _connectionString)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Dispose();
        }

        public _AdminDatabaseControlModel(string connectionString)
        {
            _connectionString = connectionString;

        }


        public int NotActivated()
        {
            int count = 0;

           using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Toplam aktif olmayan şirket sayısı
                string query = "SELECT COUNT(*) FROM Companies WHERE isAccountActivated = 0";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    count = (int)cmd.ExecuteScalar();
                }
            }
            return count;
        }




        /* ------------------------------------- DASHBOARD ------------------------------------- */
        public ModelForAdminPages Dashboard()
        {
            ModelForAdminPages modelForAdminPages = new ModelForAdminPages();

           using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();


                // Toplam Company sayısı
                string query = "SELECT COUNT(*) FROM Companies WHERE isAccountActivated = 1";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    modelForAdminPages.TotalCompanies = (int)cmd.ExecuteScalar();
                }

                // Toplam aktif olmayan şirket sayısı
                query = "SELECT COUNT(*) FROM Companies WHERE isAccountActivated = 0";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    modelForAdminPages.notActivated = (int)cmd.ExecuteScalar();
                }



                // Toplam Products sayısı
                query = "SELECT COUNT(*) FROM Products";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    modelForAdminPages.TotalProducts = (int)cmd.ExecuteScalar();
                }


                // Toplam Reviews sayısı
                query = "SELECT COUNT(*) FROM Reviews";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    modelForAdminPages.TotalReviews = (int)cmd.ExecuteScalar();
                }



                // Toplam Users sayısı
                query = "SELECT COUNT(*) FROM Users";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    modelForAdminPages.TotalUsers = (int)cmd.ExecuteScalar();
                }



                // En çok yorum alan ürün
                query = @"
            SELECT TOP 1 p.ProductId, p.CompanyId, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt, p.Category, p.Rating, p.Favorite, p.Clicked, p.isAvailable, COUNT(r.Id) AS ReviewCount
            FROM Products p
            JOIN Reviews r ON p.ProductId = r.ProductId
            GROUP BY p.ProductId, p.CompanyId, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt, p.Category, p.Rating, p.Favorite, p.Clicked, p.isAvailable
            ORDER BY ReviewCount DESC";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelForAdminPages.mostReviewedProduct = new ProductModel
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
                        }
                    }
                }


                string imageQuery = "SELECT ImageURL FROM ProductImages WHERE ProductId = @ProductId";
                using (SqlCommand cmd = new SqlCommand(imageQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductId", modelForAdminPages.mostReviewedProduct.ProductId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (modelForAdminPages.mostReviewedProduct.Images == null)
                        {
                            modelForAdminPages.mostReviewedProduct.Images = new List<string>();
                        }
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string imageUrl = reader["ImageURL"].ToString();
                                if (!string.IsNullOrWhiteSpace(imageUrl))
                                {
                                    modelForAdminPages.mostReviewedProduct.Images.Add(imageUrl);
                                }
                            }
                        }
                    }
                }




                // En çok beğenilen ürün
                query = @"
            SELECT TOP 1 ProductId, CompanyId, Name, Description, Price, Stock, CreatedAt, Category, Rating, Favorite, Clicked, isAvailable
            FROM Products
            ORDER BY Favorite DESC";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelForAdminPages.mostLikedProduct = new ProductModel
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
                        }
                    }
                }




                imageQuery = "SELECT ImageURL FROM ProductImages WHERE ProductId = @ProductId";
                using (SqlCommand cmd = new SqlCommand(imageQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductId", modelForAdminPages.mostLikedProduct.ProductId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (modelForAdminPages.mostLikedProduct.Images == null)
                        {
                            modelForAdminPages.mostLikedProduct.Images = new List<string>();
                        }
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string imageUrl = reader["ImageURL"].ToString();
                                if (!string.IsNullOrWhiteSpace(imageUrl))
                                {
                                    modelForAdminPages.mostLikedProduct.Images.Add(imageUrl);
                                }
                            }
                        }
                    }
                }



                // En çok takip edilen company
                query = @"
            SELECT TOP 1 c.Id, c.UserId, c.CompanyName, c.Description, c.Address, c.PhoneNumber, c.Email, c.isAccountActivated, c.LogoUrl, c.BannerUrl, c.TaxIDNumber, c.IBAN, c.IsHighlighted, c.CreatedAt, c.Rating, COUNT(f.Id) AS FollowerCount
            FROM Companies c
            JOIN FollowedCompanies f ON c.Id = f.CompanyId
            GROUP BY c.Id, c.UserId, c.CompanyName, c.Description, c.Address, c.PhoneNumber, c.Email, c.isAccountActivated, c.LogoUrl, c.BannerUrl, c.TaxIDNumber, c.IBAN, c.IsHighlighted, c.CreatedAt, c.Rating
            ORDER BY FollowerCount DESC";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelForAdminPages.mostFollowedCompany = new CompanyModel
                            {
                                CompanyId = reader.GetInt32(reader.GetOrdinal("Id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                isAccountActivated = reader.GetBoolean(reader.GetOrdinal("isAccountActivated")),
                                LogoUrl = reader.GetString(reader.GetOrdinal("LogoUrl")),
                                BannerUrl = reader.GetString(reader.GetOrdinal("BannerUrl")),
                                TaxIDNumber = reader.GetString(reader.GetOrdinal("TaxIDNumber")),
                                IBAN = reader.GetString(reader.GetOrdinal("IBAN")),
                                isHighlighed = reader.GetString(reader.GetOrdinal("IsHighlighted")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                            };
                        }
                    }
                }



                // En çok satın alınan ürün
                query = @"
            SELECT TOP 1 p.ProductId, p.CompanyId, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt, p.Category, p.Rating, p.Favorite, p.Clicked, p.isAvailable, COUNT(pb.ProductId) AS PurchaseCount
            FROM Products p
            JOIN ProductsBought pb ON p.ProductId = pb.ProductId
            GROUP BY p.ProductId, p.CompanyId, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt, p.Category, p.Rating, p.Favorite, p.Clicked, p.isAvailable
            ORDER BY PurchaseCount DESC";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelForAdminPages.mostPurchasedProduct = new ProductModel
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
                        }
                    }
                }



                imageQuery = "SELECT ImageURL FROM ProductImages WHERE ProductId = @ProductId";
                using (SqlCommand cmd = new SqlCommand(imageQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductId", modelForAdminPages.mostPurchasedProduct.ProductId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (modelForAdminPages.mostPurchasedProduct.Images == null)
                        {
                            modelForAdminPages.mostPurchasedProduct.Images = new List<string>();
                        }
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string imageUrl = reader["ImageURL"].ToString();
                                if (!string.IsNullOrWhiteSpace(imageUrl))
                                {
                                    modelForAdminPages.mostPurchasedProduct.Images.Add(imageUrl);
                                }
                            }
                        }
                    }
                }




                // User Registration Trends

                query = @"
                    SELECT 
                        DAY(CreatedAt) AS Day, 
                        MONTH(CreatedAt) AS Month, 
                        YEAR(CreatedAt) AS Year, 
                        COUNT(*) AS RegistrationCount
                    FROM 
                        Users
                    GROUP BY 
                        DAY(CreatedAt), 
                        MONTH(CreatedAt), 
                        YEAR(CreatedAt)
                    ORDER BY 
                        Year, 
                        Month, 
                        Day";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        modelForAdminPages.UserRegistrationTrends = new List<UserRegistrationTrend>();
                        while (reader.Read())
                        {
                            modelForAdminPages.UserRegistrationTrends.Add(new UserRegistrationTrend
                            {
                                Day = reader.GetInt32(reader.GetOrdinal("Day")),
                                Month = reader.GetInt32(reader.GetOrdinal("Month")),
                                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                                RegistrationCount = reader.GetInt32(reader.GetOrdinal("RegistrationCount"))
                            });
                        }
                    }
                }



                // en çok ürünü olan şirket
                query = @"
                    WITH CompanyProductCounts AS (
                        SELECT
                            c.Id,
                            c.CompanyName,
                            c.Description,
                            c.Address,
                            c.PhoneNumber,
                            c.Email,
                            c.isAccountActivated,
                            c.LogoUrl,
                            c.BannerUrl,
                            c.TaxIDNumber,
                            c.IBAN,
                            c.IsHighlighted,
                            c.CreatedAt,
                            c.Rating,
                            COUNT(p.ProductId) AS ProductCount
                        FROM
                            Companies c
                        LEFT JOIN
                            Products p ON c.Id = p.CompanyId
                        GROUP BY
                            c.Id, c.CompanyName, c.Description, c.Address, c.PhoneNumber, c.Email, c.isAccountActivated,
                            c.LogoUrl, c.BannerUrl, c.TaxIDNumber, c.IBAN, c.IsHighlighted, c.CreatedAt, c.Rating
                    )
                    SELECT TOP 1
                        Id,
                        CompanyName,
                        Description,
                        Address,
                        PhoneNumber,
                        Email,
                        isAccountActivated,
                        LogoUrl,
                        BannerUrl,
                        TaxIDNumber,
                        IBAN,
                        IsHighlighted,
                        CreatedAt,
                        Rating,
                        ProductCount
                    FROM
                        CompanyProductCounts
                    ORDER BY
                        ProductCount DESC";



                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelForAdminPages.CompanyWithMostProducts = new CompanyModel
                            {
                                CompanyId = reader.GetInt32(reader.GetOrdinal("Id")),
                                CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                isAccountActivated = reader.GetBoolean(reader.GetOrdinal("isAccountActivated")),
                                LogoUrl = reader.GetString(reader.GetOrdinal("LogoUrl")),
                                BannerUrl = reader.GetString(reader.GetOrdinal("BannerUrl")),
                                TaxIDNumber = reader.GetString(reader.GetOrdinal("TaxIDNumber")),
                                IBAN = reader.GetString(reader.GetOrdinal("IBAN")),
                                isHighlighed = reader.GetString(reader.GetOrdinal("IsHighlighted")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                            };

                            // En çok ürüne sahip şirketin ürün sayısını ayarlama
                            modelForAdminPages.CompanyWithMostProductsTotalProducts = reader.GetInt32(reader.GetOrdinal("ProductCount"));
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
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        modelForAdminPages.DailyRevenues = new List<DailyRevenue>();
                        while (reader.Read())
                        {
                            modelForAdminPages.DailyRevenues.Add(new DailyRevenue
                            {
                                Day = reader.GetInt32(reader.GetOrdinal("Day")),
                                Month = reader.GetInt32(reader.GetOrdinal("Month")),
                                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                                DailyRevenueAmount = reader.GetDecimal(reader.GetOrdinal("DailyRevenue"))
                            });
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
                    GROUP BY 
                        p.Category
                    ORDER BY 
                        TotalSales DESC";
                    

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                modelForAdminPages.salesByCategory.Add(new SalesByCategory
                                {
                                    Category = reader.GetString(reader.GetOrdinal("Category")),
                                    TotalSales = reader.GetInt32(reader.GetOrdinal("TotalSales"))
                                });
                            }
                        }
                    }






            }
            return modelForAdminPages;
        }



            /* ------------------------------------- COMPANIES ------------------------------------- */
            public ModelForAdminPages getAllCompanies()
            {
                ModelForAdminPages modelForAdminPages = new ModelForAdminPages();


               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string companyQuery = "SELECT * FROM Companies WHERE isAccountActivated = @IsAccountActivated";
                    using (SqlCommand companyCmd = new SqlCommand(companyQuery, connection))
                    {

                        companyCmd.Parameters.AddWithValue("@IsAccountActivated", true);
                        using (SqlDataReader reader = companyCmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                CompanyModel company = new CompanyModel
                                {
                                    CompanyId = reader.GetInt32(0),
                                    UserId = reader.GetInt32(1),
                                    CompanyName = reader.GetString(2),
                                    Description = reader.GetString(3),
                                    Address = reader.GetString(4),
                                    PhoneNumber = reader.GetString(5),
                                    Email = reader.GetString(6),
                                    isAccountActivated = reader.GetBoolean(7),
                                    LogoUrl = reader.GetString(8),
                                    BannerUrl = reader.GetString(9),
                                    TaxIDNumber = reader.GetString(10),
                                    IBAN = reader.GetString(11),
                                    isHighlighed = reader.GetString(12),
                                    CreatedAt = reader.GetDateTime(13),
                                    Rating = reader.GetInt32(14)
                                };
                                modelForAdminPages.activateCompanies.Add(company);
                            }
                        }
                    }

                    for (int i = 0; i < modelForAdminPages.activateCompanies.Count; i++)
                    {
                        // User tablosundan verileri almak için sorgu
                        string userQuery = "SELECT * FROM Users WHERE Id = @userId";
                        using (SqlCommand userCmd = new SqlCommand(userQuery, connection))
                        {
                            userCmd.Parameters.AddWithValue("@userId", modelForAdminPages.activateCompanies[i].UserId);
                            using (SqlDataReader reader = userCmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UserModel user = new UserModel
                                    {
                                        UserId = reader.GetInt32(0),
                                        Age = reader.GetInt32(1),
                                        Name = reader.GetString(2),
                                        Surname = reader.GetString(3),
                                        PhoneNumber = reader.GetString(4),
                                        PasswordHash = reader.GetString(5),
                                        Email = reader.GetString(6),
                                        Address = reader.GetString(7),
                                        Role = reader.GetString(8),
                                        Birthdate = reader.GetDateTime(9),
                                        CreatedAt = reader.GetDateTime(10)
                                    };
                                    modelForAdminPages.activateCompaniesUsers.Add(user);
                                }
                            }
                        }
                    }
                }
            return modelForAdminPages;
        }



        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return new signed companies to activate */
        public ModelForAdminPages getNotAcitavedCompanies()
        {
            ModelForAdminPages modelForAdminPages = new ModelForAdminPages();


           using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string companyQuery = "SELECT * FROM Companies WHERE isAccountActivated = @IsAccountActivated";
                using (SqlCommand companyCmd = new SqlCommand(companyQuery, connection))
                {

                    companyCmd.Parameters.AddWithValue("@IsAccountActivated", false);
                    using (SqlDataReader reader = companyCmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            CompanyModel company = new CompanyModel
                            {
                                CompanyId = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                CompanyName = reader.GetString(2),
                                Description = reader.GetString(3),
                                Address = reader.GetString(4),
                                PhoneNumber = reader.GetString(5),
                                Email = reader.GetString(6),
                                isAccountActivated = reader.GetBoolean(7),
                                LogoUrl = reader.GetString(8),
                                BannerUrl = reader.GetString(9),
                                TaxIDNumber = reader.GetString(10),
                                IBAN = reader.GetString(11),
                                isHighlighed = reader.GetString(12),
                                CreatedAt = reader.GetDateTime(13),
                                Rating = reader.GetInt32(14)
                            };
                            modelForAdminPages.activateCompanies.Add(company);
                        }
                    }
                }

                for (int i = 0; i < modelForAdminPages.activateCompanies.Count; i++)
                {
                    // User tablosundan verileri almak için sorgu
                    string userQuery = "SELECT * FROM Users WHERE Id = @userId";
                    using (SqlCommand userCmd = new SqlCommand(userQuery, connection))
                    {
                        userCmd.Parameters.AddWithValue("@userId", modelForAdminPages.activateCompanies[i].UserId);
                        using (SqlDataReader reader = userCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserModel user = new UserModel
                                {
                                    UserId = reader.GetInt32(0),
                                    Age = reader.GetInt32(1),
                                    Name = reader.GetString(2),
                                    Surname = reader.GetString(3),
                                    PhoneNumber = reader.GetString(4),
                                    PasswordHash = reader.GetString(5),
                                    Email = reader.GetString(6),
                                    Address = reader.GetString(7),
                                    Role = reader.GetString(8),
                                    Birthdate = reader.GetDateTime(9),
                                    CreatedAt = reader.GetDateTime(10)
                                };
                                modelForAdminPages.activateCompaniesUsers.Add(user);
                            }
                        }
                    }
                }
            }
            return modelForAdminPages;
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Activate account */
        public void ActivateAccount(int CompanyId)
        {
            string updateQuery = "UPDATE Companies SET isAccountActivated = @IsAccountActivated WHERE Id = @CompanyId";

           using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@IsAccountActivated", true);
                    cmd.Parameters.AddWithValue("@CompanyId", CompanyId);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                }
            }
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return all users */
        public List<UserModel> getAllUsers()
        {
            List<UserModel> users = new List<UserModel>();

            try
            {
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Users";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
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

                                users.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting users: " + ex.Message);
            }

            return users;
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Delete User */
        public void DeleteUser(int userId)
        {
            try
            {
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // SQL query to delete the user
                    string deleteUserQuery = "DELETE FROM Users WHERE Id = @UserId";
                    using (SqlCommand cmd = new SqlCommand(deleteUserQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                      
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the user: " + ex.Message);
                // Optionally rethrow or handle the exception based on your application's needs
            }
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return all products */
        public List<ProductModel> getAllProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string productQuery = "SELECT * FROM Products";
                    using (SqlCommand cmd = new SqlCommand(productQuery, connection))
                    {
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
                                products.Add(product);
                            }
                        }
                    }
                }


               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
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
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading products: " + ex.Message);
            }

            return products;
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Delete Product */
        public void DeleteProduct(int productId)
        {
            try
            {
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // SQL query to delete the user
                    string deleteUserQuery = "DELETE FROM Products WHERE ProductId = @productId";
                    using (SqlCommand cmd = new SqlCommand(deleteUserQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the product: " + ex.Message);
            }
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return all reviews */
        public List<ProductReviewModel> GetAllReviews()
        {
            List<ProductReviewModel> reviews = new List<ProductReviewModel>();

            try
            {
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Reviews";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductReviewModel review = new ProductReviewModel
                                {
                                    ReviewId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                                    Review = reader.GetString(reader.GetOrdinal("Review")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                                };

                                reviews.Add(review);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting users: " + ex.Message);
            }

            return reviews;
        }

        /* -------------------------------------------------------------------------------------------------------------- */
        /* Delete Review */
        public void DeleteReview(int reviewId)
        {
            try
            {
               using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // SQL query to delete the user
                    string deleteUserQuery = "DELETE FROM Reviews WHERE Id = @reviewId";
                    using (SqlCommand cmd = new SqlCommand(deleteUserQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@reviewId", reviewId);
                        int rowsAffected = cmd.ExecuteNonQuery();


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the user: " + ex.Message);
            }
        }



    }
}
