using Microsoft.EntityFrameworkCore;
using ProdutosApi.Context;
using ProdutosApi.Models;

namespace ProdutosApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(Category category)
        {
            _context.Categorias.Add(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Category category = _context.Categorias.Find(id);

            _context.Categorias.Remove(category);
            _context.SaveChanges();
        }

        public Category Get(int id)
        {
            return _context.Categorias.AsNoTracking().FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categorias.AsNoTracking().ToList();
        }

        public void Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<Category> GetCategoriesWithProducts()
        {
            return _context.Categorias.AsNoTracking().Include(c => c.Products).ToList();
        }
    }
}
