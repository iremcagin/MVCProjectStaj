using System.Data.SqlClient;

namespace myProject.Models
{
    public class _AdminDatabaseControlModel
    {

        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\iremc\OneDrive\Documents\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        public _AdminDatabaseControlModel() { }



        /* ------------------------------------- COMPANIES ------------------------------------- */
        public List<CombinedViewModel> getAllCompanies()
        {
            List<CombinedViewModel> combinedViewModelList = new List<CombinedViewModel>();
            CombinedViewModel combinedViewModel = new CombinedViewModel();


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


        public List<CombinedViewModel> getNotAcitavedCompanies()
        {
            List<CombinedViewModel> combinedViewModelList = new List<CombinedViewModel>();
            CombinedViewModel combinedViewModel = new CombinedViewModel();


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




    }
}
