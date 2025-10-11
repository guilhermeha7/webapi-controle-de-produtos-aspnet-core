using Microsoft.EntityFrameworkCore;
using ProdutosApi.Context;
using ProdutosApi.Pagination;
using System.Linq.Expressions;

namespace ProdutosApi.Repositories
{
    public class Repository<T> : IRepository<T> where T : class //where T : class exige que o tipo genérico seja uma classe, como Produto e Categoria
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        public IEnumerable<T> GetWithPagination(PaginationParameters paginationParams, Expression<Func<T, object>> orderBy) //Retorna uma página apenas na verdade da tabela escolhida
        {
            return _context.Set<T>()
                .AsNoTracking()
                .OrderBy(orderBy)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize).ToList();
        }

        public T GetById(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T GetByIdAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public int GetTotalItems()
        {
            return _context.Set<T>().Count();
        }

    }
}
