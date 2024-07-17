namespace myProject.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public ICollection<int> Card { get; set; }   // Card IDs
        public string Address { get; set; }
        public string Role { get; set; }  // Admin, seller, user
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<int> Reviews { get; set; }  // Review ID
        public ICollection<int> FollowedCompanies { get; set; }  // Company ID
        public ICollection<int> ProductsInBasket { get; set; }  // Product ID
        public ICollection<int> ProductsBought { get; set; }  // Product ID
    }
}
