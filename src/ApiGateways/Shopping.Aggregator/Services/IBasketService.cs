using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface IBasketService
    {
        public Task<BasketModel> GetBasket(string userName);

    }
}
