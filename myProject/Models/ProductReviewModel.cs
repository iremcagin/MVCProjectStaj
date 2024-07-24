namespace myProject.Models
{
    public class ProductReviewModel
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }  // foreign key
        public int UserId { get; set; }  // foreign key
        public int CompanyId { get; set; }  // foreign key
        public int Rating { get; set; }
        public string Review { get; set; }
        public DateTime CreatedAt { get; set; }


        // Profile için
        public List<string> Images { get; set; } = new List<string>();
        public string category { get; set; }


       
    }
}
