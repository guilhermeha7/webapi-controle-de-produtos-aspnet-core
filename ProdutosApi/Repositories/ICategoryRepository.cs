using ProdutosApi.Models;

namespace ProdutosApi.Repositories
{
    public interface ICategoryRepository
    {
        //Não é preciso colocar public na frente
        IEnumerable<Category> GetAll();
        Category Get(int id);
        void Create(Category category);
        void Update(Category category);
        void Delete(int id);
        IEnumerable<Category> GetCategoriesWithProducts();
    }
}
