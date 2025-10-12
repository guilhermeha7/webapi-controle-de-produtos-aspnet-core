using ProdutosApi.Models;
using ProdutosApi.Parameters;

namespace ProdutosApi.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> GetCategoriesWithProducts();
        (IEnumerable<Category>, int) GetFilteredCategories(CategoryParameters categoryParams);
    }
}
