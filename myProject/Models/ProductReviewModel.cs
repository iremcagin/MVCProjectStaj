namespace myProject.Models
{
    public class ProductReviewModel
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }  // foreign key
        public int UserId { get; set; }  // foreign key
        public int Rating { get; set; }
        public string Review { get; set; }
        public DateTime CreatedAt { get; set; }

       
    }
}
