using System.ComponentModel.DataAnnotations;

namespace myProject.Models
{
    public class CompanyModel
    {
        public int CompanyId { get; set; }
        public int UserId { get; set; }  // foreign key
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string LogoUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Rating { get; set; }
        public ICollection<int> ProductsList { get; set; }   // ProductID
        public ICollection<int> FollowersList { get; set; }  // UserID


    }
}
