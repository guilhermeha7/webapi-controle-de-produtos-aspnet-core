using Microsoft.EntityFrameworkCore;
using ProdutosApi.Context;
using ProdutosApi.Models;
using ProdutosApi.Parameters;
using ProdutosApi.Parameters.Enums;

namespace ProdutosApi.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetFilteredProducts(ProductParameters productParams)
        {
            // 1. Começa com a tabela
            var query = _context.Produtos.AsNoTracking().AsQueryable();

            // 2. Aplicar filtros
            if (!string.IsNullOrWhiteSpace(productParams.Name)) //Se o campo de filtro por nome NÃO estiver nulo
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{productParams.Name}%")); //Retorna todos os produtos onde o Name do produto contenha o que o usuário digitou. Os % permitem ter qualquer coisa antes ou depois.
                //query = query.Where(p => p.Name.Contains(productParams.Name));                                                                                //query = query.Where(p => p.Name.Contains(productParams.Name));
            if (productParams.MinPrice.HasValue) //Se MinPrice tem algum valor 
                query = query.Where(p => p.Price >= productParams.MinPrice.Value); //Realiza o filtro

            if (productParams.MaxPrice.HasValue) //Se MaxPrice tem algum valor
                query = query.Where(p => p.Price <= productParams.MaxPrice.Value); //Realiza o filtro

            // 3. Ordenação
            query = (productParams.SortBy, productParams.SortDirection) switch //Dependendo dos valores das variáveis SortBy e SortDirection serão realizadas ações   
            {
                (ProductSort.Name, SortDirection.Asc) => query.OrderBy(p => p.Name).ThenBy(p => p.Id),
                (ProductSort.Name, SortDirection.Desc) => query.OrderByDescending(p => p.Name).ThenBy(p => p.Id),
                (ProductSort.Price, SortDirection.Asc) => query.OrderBy(p => p.Price).ThenBy(p => p.Id),
                (ProductSort.Price, SortDirection.Desc) => query.OrderByDescending(p => p.Price).ThenBy(p => p.Id),
                (ProductSort.RegistrationDate, SortDirection.Asc) => query.OrderBy(p => p.RegistrationDate).ThenBy(p => p.Id),
                (ProductSort.RegistrationDate, SortDirection.Desc) => query.OrderByDescending(p => p.RegistrationDate).ThenBy(p => p.Id),
                _ => query.OrderBy(p => p.Id)
            };

            //Paginação
            return query.Skip((productParams.PageNumber - 1) * productParams.PageSize)
                .Take(productParams.PageSize)
                .ToList();
        }

        public IEnumerable<Product> GetProductsFromCategoryId(int id)
        {
            return _context.Produtos.Where(p => p.CategoryId == id);
        }
    }
}
