using System.Data.SqlClient;

namespace myProject.Models
{
    public class _AdminDatabaseControlModel
    {

        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\iremc\OneDrive\Documents\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        public _AdminDatabaseControlModel() { }



        /* ------------------------------------- COMPANIES ------------------------------------- */
        public List<ModelForAdminPages> getAllCompanies()
        {
            List<ModelForAdminPages> combinedViewModelList = new List<ModelForAdminPages>();
            ModelForAdminPages combinedViewModel = new ModelForAdminPages();


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();


                // Company tablosundan verileri almak için sorgu
                string companyQuery = "SELECT * FROM Companies";
                using (SqlCommand companyCmd = new SqlCommand(companyQuery, conn))
                {
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
                            combinedViewModel.Company = company;
                            combinedViewModelList.Add(combinedViewModel);

                        }
                    }
                }

                for (int i = 0; i < combinedViewModelList.Count; i++) {
                    // User tablosundan verileri almak için sorgu
                    string userQuery = "SELECT * FROM Users WHERE Id = @userId";
                    using (SqlCommand userCmd = new SqlCommand(userQuery, conn))
                    {
                        userCmd.Parameters.AddWithValue("@userId", combinedViewModelList[i].Company.UserId);
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
                                combinedViewModelList[i].User = user;
                            }
                        }
                    }
                }


            }
            return combinedViewModelList;
        }



        /* -------------------------------------------------------------------------------------------------------------- */
        /* Return new signed companies to activate */
        public List<ModelForAdminPages> getNotAcitavedCompanies()
        {
            List<ModelForAdminPages> combinedViewModelList = new List<ModelForAdminPages>();
            ModelForAdminPages combinedViewModel = new ModelForAdminPages();


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string companyQuery = "SELECT * FROM Companies WHERE isAccountActivated = @IsAccountActivated";
                using (SqlCommand companyCmd = new SqlCommand(companyQuery, conn))
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
                            combinedViewModel.Company = company;
                            combinedViewModelList.Add(combinedViewModel);

                        }
                    }
                }

                for (int i = 0; i < combinedViewModelList.Count; i++)
                {
                    // User tablosundan verileri almak için sorgu
                    string userQuery = "SELECT * FROM Users WHERE Id = @userId";
                    using (SqlCommand userCmd = new SqlCommand(userQuery, conn))
                    {
                        userCmd.Parameters.AddWithValue("@userId", combinedViewModelList[i].Company.UserId);
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
                                combinedViewModelList[i].User = user;
                            }
                        }
                    }
                }


            }
            return combinedViewModelList;
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Activate account */
        public void ActivateAccount(int CompanyId)
        {
            string updateQuery = "UPDATE Companies SET isAccountActivated = @IsAccountActivated WHERE Id = @CompanyId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@IsAccountActivated", true);
                    cmd.Parameters.AddWithValue("@CompanyId", CompanyId);

                    conn.Open();
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Users";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // SQL query to delete the user
                    string deleteUserQuery = "DELETE FROM Users WHERE Id = @UserId";
                    using (SqlCommand cmd = new SqlCommand(deleteUserQuery, conn))
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
        /* Delete Product */
        public void DeleteProduct(int productId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // SQL query to delete the user
                    string deleteUserQuery = "DELETE FROM Products WHERE ProductId = @productId";
                    using (SqlCommand cmd = new SqlCommand(deleteUserQuery, conn))
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Reviews";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // SQL query to delete the user
                    string deleteUserQuery = "DELETE FROM Reviews WHERE Id = @reviewId";
                    using (SqlCommand cmd = new SqlCommand(deleteUserQuery, conn))
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
