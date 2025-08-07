using System.Linq.Expressions;


namespace Repositories.Contracts
{
    public interface IRepositoryBase<T> where T : class // T for type(class)
    {
        // CRUD
        IQueryable<T> FindAll(bool trackChanges);

        //IQueryable<T> FindAllStocks(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T,bool>> expresion, bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
