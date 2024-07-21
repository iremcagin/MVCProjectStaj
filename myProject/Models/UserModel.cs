namespace myProject.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public int Age { get; set; } = 0;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }  // Admin, seller, user
        public DateTime Birthdate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public List<int> CreditCards { get; set; } = new List<int>();   // Card IDs
        public List<int> Reviews { get; set; } = new List<int>();  // Review ID
        public List<int> FollowedCompanies { get; set; } = new List<int>();  // Company ID
        public List<int> ProductsInBasket { get; set; } = new List<int>();  // Product ID
        public List<int> ProductsBought { get; set; } = new List<int>();  // Product ID
    }
}
