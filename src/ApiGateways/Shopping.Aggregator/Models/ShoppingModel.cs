namespace Shopping.Aggregator.Models
{
    public class ShoppingModel
    {
        public string UserName { get; set; }
        public BasketModel BasketWithProducts { get; set; }
        public IList<OrderResponseModel> Orders { get; set; } 
    }
}
