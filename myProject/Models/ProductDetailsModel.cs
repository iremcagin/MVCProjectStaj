namespace myProject.Models
{
    public class ProductDetailsModel
    {
        public CompanyModel Company { get; set; }
        public UserModel User { get; set; } /* Company owner bilgileri */
        public ProductModel Product { get; set; }
        public List<string> ProductImages { get; set; } = new List<string>();

        public List<ProductReviewModel> ProductReviews { get; set; } = new List<ProductReviewModel>();
        public List<UserModel> ReviewedUsers { get; set; } = new List<UserModel>();


         
    }
}
