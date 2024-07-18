using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace myProject.Models
{
    public class CategoriesModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string MainCategory { get; set; }



        public CategoriesModel() { }

        /* Databasede bulununan tüm eşya sınıfı kategorileri okur. Koltuk masa gibi. */
        public List<CategoriesModel> GetAllFurnitureCategories()
        {
            List<CategoriesModel> categories = new List<CategoriesModel>();
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\iremc\OneDrive\Documents\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30";

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
                                CategoriesModel category = new CategoriesModel
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

        /* Databasede bulununan tüm dekorasyon sınıfı kategorileri okur. Perde halı gibi. */
        public List<CategoriesModel> GetAllDecorationCategories()
        {
            List<CategoriesModel> categories = new List<CategoriesModel>();
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
                                CategoriesModel category = new CategoriesModel
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
