using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        public CatalogService(HttpClient httpClient)
        {
            _httpClient =httpClient;
        }

        public async Task<IList<CatalogModel>> GetProductByCategory(string category)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Catalog/GetProductByCategory/{category}");
            return await response.ReadContentAs<IList<CatalogModel>>();
        }

        public async Task<CatalogModel> GetProductById(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Catalog/{id}");
            return await response.ReadContentAs<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetProducts()
        {
            var response = await _httpClient.GetAsync("/api/v1/Catalog");
            return await response.ReadContentAs<IEnumerable<CatalogModel>>();
        }
    }
}
