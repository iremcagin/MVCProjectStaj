using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace myProject.Models
{
    public class _SellerDatabaseControlModel
    {
       
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\iremc\OneDrive\Documents\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30";


        public _SellerDatabaseControlModel() {
            
         
        }


        /* --------------------------------------------------- Products Page --------------------------------------------------- */
        public void saveProductToDatabase(ProductModel newProduct)
        {
           
            int productId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                Console.WriteLine("Connection Opened.");

                try
                {
                    string query = @"INSERT INTO Products (CompanyId, Name, Description, Price, Stock, CreatedAt, Category, Rating, Favorite, isAvailable)
                             VALUES (@CompanyId, @Name, @Description, @Price, @Stock, @CreatedAt, @Category, @Rating, @Favorite, @isAvailable);
                             SELECT SCOPE_IDENTITY();"; // SCOPE_IDENTITY ile eklenen son kaydın Id değerini alır
                   
                    SqlCommand cmd = new SqlCommand(query, conn);
                    
                    // Parametreleri ayarla
                    cmd.Parameters.AddWithValue("@CompanyId", newProduct.CompanyId);
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

                            SqlCommand cmd = new SqlCommand(query, conn);

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


        public List<ProductModel> getAllProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
                // İlk bağlantıyı açarak ürünleri okuyun
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
