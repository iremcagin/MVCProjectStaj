namespace myProject.Models
{
    public class ProductDetailsModel
    {
        public CompanyModel Company { get; set; }
        public UserModel User { get; set; } /* Company owner bilgileri */
        public ProductModel Product { get; set; }
        public List<string> ProductImages { get; set; }

        public List<ProductReviewModel> ProductReviews { get; set; }
        public List<UserModel> ReviewedUsers { get; set; }


         
    }
}
