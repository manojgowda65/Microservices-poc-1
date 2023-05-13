namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string UserName { get; set; }

        public IList<BasketItemExtendedModel> Items { get; set; }

        public BasketModel(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                foreach (var item in Items)
                {
                    totalPrice += item.Price * item.Quantity;
                }
                return totalPrice;
            }
        }
    }
}
