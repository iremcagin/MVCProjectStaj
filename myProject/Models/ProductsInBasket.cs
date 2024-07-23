namespace myProject.Models
{
    public class ProductsInBasket
    {
        public List<ProductInBasket> Products { get; set; } = new List<ProductInBasket>();
        public int UserId { get; set; }

       
    }
}
