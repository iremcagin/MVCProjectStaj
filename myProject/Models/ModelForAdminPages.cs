using myProject.Models;

namespace myProject.Models
{
    public class ModelForAdminPages
    {
        public UserModel User { get; set; }
        public CompanyModel Company { get; set; }

        public IFormFile Logo { get; set; }
        public IFormFile Banner { get; set; }



        // Dashboard için
        public int notActivated { get; set; }
        public int TotalCompanies { get; set; }
        public int TotalProducts { get; set; }
        public int TotalReviews { get; set; }
        public int TotalUsers { get; set; }
        public ProductModel mostLikedProduct { get; set; } = new ProductModel();
        public ProductModel mostReviewedProduct { get; set; } = new ProductModel();
        public ProductModel mostPurchasedProduct { get; set; } = new ProductModel();
        public CompanyModel mostFollowedCompany { get; set; } = new CompanyModel();

        public List<UserRegistrationTrend> UserRegistrationTrends { get; set; } = new List<UserRegistrationTrend>();
        public CompanyModel CompanyWithMostProducts { get; set; } = new CompanyModel();
        public int CompanyWithMostProductsTotalProducts { get; set; }

        public List<DailyRevenue> DailyRevenues = new List<DailyRevenue>();
        public List<SalesByCategory> salesByCategory { get; set; } = new List<SalesByCategory>();





        // Users Page için
        public List<UserModel> users { get; set; } = new List<UserModel>();


        // Products Page için
        public List<ProductModel> products { get; set; } = new List<ProductModel>();



        // Reviews Page için
        public List<ProductReviewModel> reviews { get; set; } = new List<ProductReviewModel>();


        //  Not acitaved accounts için
        public List<CompanyModel> activateCompanies { get; set; }= new List<CompanyModel> { };
        public List<UserModel> activateCompaniesUsers { get; set; } = new List<UserModel>();

    }

}

