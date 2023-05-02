using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;
        public ProductRepository(ICatalogContext context)
        {
            _catalogContext = context??throw new ArgumentNullException(nameof(context));
        }

        public async Task<Product> AddNewProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult result = await _catalogContext.Products.DeleteOneAsync(filterDefinition);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IList<Product>> GetProductsByCategory(string category)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await _catalogContext.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await  _catalogContext.Products.Find(x=>x.Id== id).FirstOrDefaultAsync();
        }

        public async Task<IList<Product>> GetProducts()
        {
            return await _catalogContext.Products.Find(x=>true).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var result = await _catalogContext
                                        .Products
                                        .ReplaceOneAsync(filter: p => p.Id==product.Id,replacement: product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
