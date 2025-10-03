using ProdutosApi.Models;

namespace ProdutosApi.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsFromCategoryId(int id);
    }
}
