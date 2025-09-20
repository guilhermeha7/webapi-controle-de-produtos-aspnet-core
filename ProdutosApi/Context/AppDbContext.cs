using Microsoft.EntityFrameworkCore;
using ProdutosApi.Models;

namespace ProdutosApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) //"ctor + enter" cria um construtor vazio
        {
            
        }

        public DbSet<Product> Produtos { get; set; }
        public DbSet<Category> Categorias { get; set; }
    }
}
