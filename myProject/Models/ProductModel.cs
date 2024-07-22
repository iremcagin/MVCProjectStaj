using System.Reflection;

namespace myProject.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public int CompanyId { get; set; } = 0; // foreign key
        public List<string> Images { get; set; } = new List<string>();
        public string Name { get; set; }
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        public int Stock { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public String Category { get; set; }

        public double Rating { get; set; } = 0;
        public int Favorite { get; set; } = 0;
        public int Clicked { get; set; } = 0;
        public string isAvailable { get; set; }
        public int Sold { get; set; } = 0;
        public List<ProductReviewModel> Reviews { get; set; } = new List<ProductReviewModel>();  // reviewID





    }
}
