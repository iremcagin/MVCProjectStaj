using System.Reflection;

namespace myProject.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public int CompanyId { get; set; }  // foreign key
        public List<string> Images { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        public int Stock { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public String Category { get; set; }

        public float Rating { get; set; }
        public int Favorite { get; set; } = 0;
        public string isAvailable { get; set; } = "true";
        public List<ProductReviewModel> Reviews { get; set; }  // reviewID



        public static void saveData(ProductModel newProduct)
        {
            Console.WriteLine("Images:");
            foreach (var image in newProduct.Images)
            {
                Console.WriteLine($"- {image}");
            }
        }
    }
}
