using ProdutosApi.Models;
using ProdutosApi.Parameters;

namespace ProdutosApi.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsFromCategoryId(int id);
        IEnumerable<Product> GetFilteredProducts(ProductParameters productParams);
    }
}
