using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface IOrderingService
    {
        public Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);

    }
}
