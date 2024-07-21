using System.ComponentModel.DataAnnotations;

namespace myProject.Models
{
    public class CompanyModel
    {
        public int CompanyId { get; set; }
        public int UserId { get; set; }  // foreign key
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool isAccountActivated { get; set; } = false;
        public string LogoUrl { get; set; }
        public string BannerUrl { get; set; }
        public string TaxIDNumber { get; set; }
        public string IBAN { get; set; }

        public string isHighlighed { get; set; } = "false";
       

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Rating { get; set; } = 0;


        public List<int> ProductsList { get; set; } = new List<int>();  // ProductID
        public List<int> FollowersList { get; set; } = new List<int>(); // UserID


    }
}
