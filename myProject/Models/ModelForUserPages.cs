using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace myProject.Models
{
    public class ModelForUserPages
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\iremc\OneDrive\Documents\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string MainCategory { get; set; }


        // Tüm sayfalar için
        public static List<int> productsLiked = new List<int>();


        // Ana sayfa
        public List<ProductModel> mostClickedProducts = new List<ProductModel>();
        public List<CompanyModel> mostClickedProductsCompanyInfo = new List<CompanyModel>();

        public List<ProductModel> newestProducts = new List<ProductModel>();

        public List<ProductReviewModel> randomlySelectedReviewModel = new List<ProductReviewModel>();
        public List<ProductModel> randomlySelectedProductModel = new List<ProductModel>();


        // User Product Details için
        public List<ProductModel>  recommendatitons = new List<ProductModel>();


        // User Basket için
        public static decimal totalPrice = 0;
        public ProductsInBasket productsInBasket = new ProductsInBasket();
        public ProductDetailsModel productDetailsModel = new ProductDetailsModel();
        public ProductsInBasket productsDeletedFromBasket = new ProductsInBasket();

        // User profile için
        public UserModel userProfile = new UserModel();
        public List<ProductReviewModel> productReviews = new List<ProductReviewModel>();
        public List<CardModel> creditCards { get; set; } = new List<CardModel>();
        public List<CompanyModel> followedCompanies { get; set; } = new List<CompanyModel>();
        public List<ProductModel> productsBought { get; set; } = new List<ProductModel>();


        // Home sayfası için
        public static List<CompanyModel> companies {  get; set; } = new List<CompanyModel>();


        // Alt Linkler için
        public List<ProductModel> productsByCategory { get; set; } = new List<ProductModel>();
        public string subcategory {  get; set; }
        

        // Company Sayfası için
        public CompanyModel companyDetails { get; set; }
        public List<ProductModel> companyProducts { get; set; } = new List<ProductModel>();
        public bool isFollowing;



        // Favorites için
        public List<ProductModel> favoriteProducts { get; set; } = new List<ProductModel>();





        public ModelForUserPages() { }



        /* -------------------------------------------------------------------------------------------------------------- */
        /* CalculatePrice */
        public static decimal CalculatePrice(int? userId)
        {

            if (!userId.HasValue)
            {
                throw new ArgumentException("User ID cannot be null.");
            }

            string query = @"
            SELECT SUM(p.Price * pb.Count) AS TotalPrice
            FROM ProductsInBasket pb
            INNER JOIN Products p ON pb.ProductId = p.ProductId
            WHERE pb.UserId = @UserId
            GROUP BY pb.UserId";

            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\iremc\\OneDrive\\Documents\\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId.Value);

                    var result = cmd.ExecuteScalar();
                    totalPrice = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }

            return totalPrice;
        }



     

        /* -------------------------------------------------------------------------------------------------------------- */
        /* Databasede bulununan tüm eşya sınıfı kategorileri okur. Koltuk masa gibi. */
        public List<ModelForUserPages> GetAllFurnitureCategories()
        {
            List<ModelForUserPages> categories = new List<ModelForUserPages>();
           
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, CategoryName, MainCategory FROM Categories Where MainCategory = 'Furniture' ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ModelForUserPages category = new ModelForUserPages
                                {
                                    Id = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1),
                                    MainCategory = reader.GetString(2)
                                };
                                categories.Add(category);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return categories;
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Databasede bulununan tüm dekorasyon sınıfı kategorileri okur. Perde halı gibi. */
        public List<ModelForUserPages> GetAllDecorationCategories()
        {
            List<ModelForUserPages> categories = new List<ModelForUserPages>();
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\iremc\OneDrive\Documents\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, CategoryName, MainCategory FROM Categories  Where MainCategory = 'Decoration'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ModelForUserPages category = new ModelForUserPages
                                {
                                    Id = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1),
                                    MainCategory = reader.GetString(2)
                                };
                                categories.Add(category);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return categories;
        }
    }
}
