using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IOrderingService _orderingService;
        private readonly IBasketService _basketService;
        public ShoppingController(
                ICatalogService catalogService,
                IOrderingService orderingService,
                IBasketService basketService
            )
        { 
            _basketService = basketService;
            _catalogService = catalogService;
            _orderingService = orderingService;
        }

        [HttpGet("{userName}",Name ="GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), 200)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            ShoppingModel shopping = new ShoppingModel();
            shopping.UserName = userName;
            //shopping.BasketWithProducts = await _

            //1. get basket details by username
            //   iterate basket items and consume catalog api by product id to get product details

            //2. get orders by userName
            BasketModel basket = await _basketService.GetBasket(shopping.UserName);
            if (basket != null && basket.Items!=null)
            {
                foreach(var item in basket.Items)
                {
                    var productInfo = await _catalogService.GetProductById(item.ProductId);
                    if (productInfo != null)
                    {
                        item.ProductName = productInfo.Name;
                        item.Category = productInfo.Category;
                        item.Summary = productInfo.Summary;
                        item.ImageFile = productInfo.ImageFile;
                        item.Description = productInfo.Description;
                    }
                }
            }
            shopping.BasketWithProducts = basket;
            var orders = await _orderingService.GetOrdersByUserName(userName);
            if (orders != null)
                shopping.Orders = orders.ToList();


            return Ok(shopping);
           
        }
    }
}
