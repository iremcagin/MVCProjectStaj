using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace myProject.Models
{
    public class DatabaseControlModel
    {
        private SqlConnection conn;
        //private SqlDataReader reader;


        public DatabaseControlModel() {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\iremc\OneDrive\Documents\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30";
            conn = new SqlConnection(connectionString);
         
        }


        /* --------------------------------------------------- Products Page --------------------------------------------------- */
        public void saveProductToDatabase(ProductModel newProduct)
        {
           
            int productId = 0;

            using (conn)
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

            using (conn)
            {
                try
                {
                    conn.Open();
                    string query = @"
                                    SELECT p.ProductId, p.CompanyId, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt, p.Category, p.Rating, p.Favorite, p.isAvailable, pi.ImageURL
                                    FROM Products p
                                    LEFT JOIN ProductImages pi ON p.ProductId = pi.ProductId";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            SqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                int productId = (int)reader["ProductId"];
                                ProductModel product = products.FirstOrDefault(p => p.ProductId == productId);
                                if (product == null)
                                {
                                    product = new ProductModel
                                    {
                                        ProductId = productId,
                                        CompanyId = (int)reader["CompanyId"],
                                        Name = reader["Name"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        Price = (decimal)reader["Price"],
                                        Stock = (int)reader["Stock"],
                                        CreatedAt = (DateTime)reader["CreatedAt"],
                                        Category = reader["Category"].ToString(),
                                        Rating = (double)reader["Rating"],
                                        Favorite = (int)reader["Favorite"],
                                        isAvailable = reader["isAvailable"].ToString(),
                                        Images = new List<string>()
                                    };
                                    products.Add(product);
                                }

                                // Her bir resmi Images listesine ekle
                                if (reader["ImageURL"] != DBNull.Value)
                                {
                                    product.Images.Add(reader["ImageURL"].ToString());
                                }
                            }
                            reader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error while reading products: " + ex.Message);
                    }
                }



            return products;
        }



    }
}
