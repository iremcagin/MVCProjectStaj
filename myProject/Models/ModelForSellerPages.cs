namespace myProject.Models
{
    public class ModelForSellerPages
    {

        // Products Page için
        public List<ProductModel> products { get; set; } = new List<ProductModel>();
        public List<string> categories { get; set; } = new List<string>();

    }
}
