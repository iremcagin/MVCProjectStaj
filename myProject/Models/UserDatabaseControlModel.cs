using System.Data.SqlClient;

namespace myProject.Models
{
    public class UserDatabaseControlModel
    {

        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\iremc\OneDrive\Documents\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30";


        public UserDatabaseControlModel()
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




    }
}
