using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using System.Reflection;

namespace myProject.Models
{
    public class _LoginDatabaseControlModel
    {

        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\iremc\OneDrive\Documents\myProjectDatabase.mdf;Integrated Security=True;Connect Timeout=30";


        public _LoginDatabaseControlModel() { }


        /* ------------------------------------- USER SIGN UP ------------------------------------- */
        public void UserSignUp(UserModel model)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Calculate age from Birthdate
                int age = CalculateAge(model.Birthdate);
                model.Age = age;


                // Hash the password
                var passwordHasher = new PasswordHasher<UserModel>();
                model.PasswordHash = passwordHasher.HashPassword(model, model.PasswordHash);


                string query = @"
                INSERT INTO Users 
                (Age, Name, Surname, PasswordHash, Email, PhoneNumber, Address, Role, Birthdate,CreatedAt) 
                VALUES 
                (@Age, @Name, @Surname, @PasswordHash, @Email, @PhoneNumber, @Address, @Role, @Birthdate ,@CreatedAt)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Age", model.Age);
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Surname", model.Surname);
                    cmd.Parameters.AddWithValue("@PasswordHash", model.PasswordHash);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", model.Address);
                    cmd.Parameters.AddWithValue("@Role", "user");
                    cmd.Parameters.AddWithValue("@Birthdate", model.Birthdate);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }

            }


        }
        public int CalculateAge(DateTime birthdate)
        {
            //Console.WriteLine(birthdate);
           // Console.WriteLine(DateTime.Now.Year);

            int age = DateTime.Now.Year - birthdate.Year;
            if (DateTime.Now.DayOfYear < birthdate.DayOfYear)
            {
                age--;
            }
            return age;
        }




        /* ------------------------------------- USER LOGIN ------------------------------------- */
        public UserModel Login(UserModel model)
        {
            UserModel loggedUser = new UserModel();



            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                SELECT * FROM Users 
                WHERE Email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", model.Email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var passwordHasher = new PasswordHasher<UserModel>();
                            var storedPasswordHash = reader["PasswordHash"].ToString();

                            var result = passwordHasher.VerifyHashedPassword(model, storedPasswordHash, model.PasswordHash);

                            if (result == PasswordVerificationResult.Success)
                            {
                                loggedUser.Role = reader["Role"].ToString();
                                loggedUser.UserId = Convert.ToInt32(reader["Id"]);
                            }
                            else
                            {
                                throw new Exception("Invalid email or password.");
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid email or password.");
                        }
                    }
                }
            }
            return loggedUser;
        }




        /* ------------------------------------- COMPANY SIGN UP ------------------------------------- */
        public void CompanySignUp(CombinedViewModel model)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Calculate age from Birthdate
                int age = CalculateAge(model.User.Birthdate);
                model.User.Age = age;

                // Hash the password
                var passwordHasher = new PasswordHasher<UserModel>();
                model.User.PasswordHash = passwordHasher.HashPassword(model.User, model.User.PasswordHash);

                // Insert User and get UserId
                string query = @"
                INSERT INTO Users 
                (Age, Name, Surname, PasswordHash, Email, PhoneNumber, Address, Role, Birthdate, CreatedAt) 
                VALUES 
                (@Age, @Name, @Surname, @PasswordHash, @Email, @PhoneNumber, @Address, @Role, @Birthdate, @CreatedAt);
                SELECT CAST(scope_identity() AS int);"; // Get the last inserted UserId

                int userId;

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Age", model.User.Age);
                    cmd.Parameters.AddWithValue("@Name", model.User.Name);
                    cmd.Parameters.AddWithValue("@Surname", model.User.Surname);
                    cmd.Parameters.AddWithValue("@PasswordHash", model.User.PasswordHash);
                    cmd.Parameters.AddWithValue("@Email", model.User.Email); // Fixed from model.Company.Email to model.User.Email
                    cmd.Parameters.AddWithValue("@PhoneNumber", model.User.PhoneNumber); // Fixed from model.Company.PhoneNumber to model.User.PhoneNumber
                    cmd.Parameters.AddWithValue("@Address", model.Company.Address);
                    cmd.Parameters.AddWithValue("@Role", "seller");
                    cmd.Parameters.AddWithValue("@Birthdate", model.User.Birthdate);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    // Execute the command and get the UserId
                    userId = (int)cmd.ExecuteScalar();
                }

                // Insert Company with the obtained UserId
                query = @"
                INSERT INTO Companies 
                (UserId, CompanyName, Description, Address, PhoneNumber, Email, isAccountActivated, LogoUrl, BannerUrl, TaxIDNumber, IBAN, IsHighlighted, CreatedAt, Rating) 
                VALUES 
                (@UserId, @CompanyName, @Description, @Address, @PhoneNumber, @Email, @isAccountActivated, @LogoUrl, @BannerUrl, @TaxIDNumber, @IBAN, @IsHighlighted, @CreatedAt, @Rating)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@CompanyName", model.Company.CompanyName);
                    cmd.Parameters.AddWithValue("@Description", model.Company.Description);
                    cmd.Parameters.AddWithValue("@Address", model.Company.Address);
                    cmd.Parameters.AddWithValue("@PhoneNumber", model.User.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", model.User.Email);
                    cmd.Parameters.AddWithValue("@isAccountActivated", false); // Default value
                    cmd.Parameters.AddWithValue("@LogoUrl", model.Company.LogoUrl);
                    cmd.Parameters.AddWithValue("@BannerUrl", model.Company.BannerUrl);
                    cmd.Parameters.AddWithValue("@TaxIDNumber", model.Company.TaxIDNumber);
                    cmd.Parameters.AddWithValue("@IBAN", model.Company.IBAN);
                    cmd.Parameters.AddWithValue("@IsHighlighted", "false"); // Default value
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Rating", 0); // Default value

                    cmd.ExecuteNonQuery();
                }
            }
        }





        /* ------------------------------------- COMPANY LOGIN ------------------------------------- */
        public bool CheckIfAccountActivated(int UserId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT IsAccountActivated FROM Companies WHERE UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToBoolean(result);
                    }
                }
            }

            return false;
        }




    }
}
