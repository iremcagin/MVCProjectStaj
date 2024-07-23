namespace myProject.Models
{
    public class ProductInBasket
    {
        public int Id { get; set; }  
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int ProductId { get; set; }

        public int count = 0;

        public ProductModel Product { get; set; } = new ProductModel();
        public List<string> Images { get; set; } = new List<string>();

    }
}
