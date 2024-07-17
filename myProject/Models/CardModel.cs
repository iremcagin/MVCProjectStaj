namespace myProject.Models
{
    public class CardModel
    {
        public int CardId { get; set; }
        public int UserId { get; set; }  // foreign key

        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpirationDate { get; set; }  // Format: MM/YY
        public string CVV { get; set; }
    }
}
