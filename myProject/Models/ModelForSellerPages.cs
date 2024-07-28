namespace myProject.Models
{
    public class ModelForSellerPages
    {
        // Navbar için
        public CompanyModel Company { get; set; } = new CompanyModel();
        public UserModel User { get; set; } = new UserModel();



        // Dashboard için
        public int TotalProducts { get; set; } = 0;
        public int TotalReviews { get; set; } = 0;
        public int TotalUsers { get; set; } = 0;
        public ProductModel mostLikedProduct { get; set; } = new ProductModel();
        public ProductModel mostReviewedProduct { get; set; } = new ProductModel();
        public ProductModel mostPurchasedProduct { get; set; } = new ProductModel();
        public ProductModel mostClickedProduct { get; set; } = new ProductModel();

        public int TotalSalesCount { get; set; } = 0;
        public decimal TotalRevenue { get; set; } = 0;
        public decimal AverageRating { get; set; } = 0;


        public List<DailySalesModel> DailySales = new List<DailySalesModel>();
        public List<DailyRevenue> DailyRevenues = new List<DailyRevenue>();
        public List<SalesByCategory> salesByCategory { get; set; } = new List<SalesByCategory>();




        // Products Page için
        public List<ProductModel> products { get; set; } = new List<ProductModel>();
        public List<string> categories { get; set; } = new List<string>();


        // Orders Page için
        public List<ProductModel> orders { get; set; } = new List<ProductModel>();


        //Customers page için
        public List<UserModel> customers { get; set; } = new List<UserModel>();


        //Followers page için
        public List<UserModel> followers { get; set; } = new List<UserModel>();






    }
}
