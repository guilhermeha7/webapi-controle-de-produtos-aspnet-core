using Microsoft.EntityFrameworkCore;
using ProdutosApi.Context;
using ProdutosApi.Models;

namespace ProdutosApi.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository 
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Category> GetCategoriesWithProducts()
        {
            return _context.Categorias.Include(c => c.Products).ToList();
        }
    }
}
