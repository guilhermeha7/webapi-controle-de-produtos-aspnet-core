using Microsoft.EntityFrameworkCore;
using ProdutosApi.Context;
using ProdutosApi.Models;
using ProdutosApi.Parameters;
using ProdutosApi.Parameters.Enums;

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

        public (IEnumerable<Category>, int) GetFilteredCategories(CategoryParameters categoryParams)
        {
            //1. Iniciar a construção da consulta ao banco de dados
            var query = _context.Categorias.AsNoTracking().AsQueryable();

            //2. Aplicando filtros escolhidos pelo usuário
            if (!string.IsNullOrEmpty(categoryParams.Name))
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{categoryParams.Name}%"));

            //3. Aplicando ordenação escolhida pelo usuário
            query = (categoryParams.SortBy, categoryParams.SortDirection) switch //Dependendo dos valores das variáveis SortBy e SortDirection serão realizadas ações   
            {
                (CategorySort.Name, SortDirection.Asc) => query.OrderBy(c => c.Name).ThenBy(c => c.Id),
                (CategorySort.Name, SortDirection.Desc) => query.OrderByDescending(c => c.Name).ThenBy(c => c.Id),
                _ => query.OrderBy(c => c.Id)
            };

            //4. Cálculo dos itens totais da consulta
            var count = query.Count();

            //5.Paginação
            var filteredCategoriesPage = query.Skip((categoryParams.PageNumber - 1) * categoryParams.PageSize)
                .Take(categoryParams.PageSize)
                .ToList();

            return (filteredCategoriesPage, count);

        }
    }
}
