namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public string UserName { get; set; }
        public List<ShoppingCartItemResponse> Items { get; set; }
        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = Items.Sum(x => x.Quantity * x.Price);
                return totalPrice;
            }
        }

        public ShoppingCartResponse() { }

        public ShoppingCartResponse(string userName)
        {
            UserName = userName;
        }
    }
}
