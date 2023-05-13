using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface ICatalogService
    {
       Task<IEnumerable<CatalogModel>> GetProducts();
       Task<CatalogModel> GetProductById(string id);
        Task<IList<CatalogModel>> GetProductByCategory(string category);
    }
}
