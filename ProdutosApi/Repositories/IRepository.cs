using ProdutosApi.Pagination;
using System.Linq.Expressions;

namespace ProdutosApi.Repositories
{
    public interface IRepository<T> where T : class
    {
        //Não é preciso colocar public na frente
        T GetById(Expression<Func<T, bool>> predicate); //Func guarda um método (é um delegate portanto) que, nesse caso, recebe um objeto T e retorna um boolean com base no predicado  
        T GetByIdAsNoTracking(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetWithPagination(PaginationParameters paginationParams, Expression<Func<T, object>> orderBy);
        int GetTotalItems();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
