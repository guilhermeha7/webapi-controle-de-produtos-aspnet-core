using ProdutosApi.Models;

namespace ProdutosApi.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> GetCategoriesWithProducts();
    }
}
