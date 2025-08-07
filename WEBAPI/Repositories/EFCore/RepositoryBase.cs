using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.DataTransferObject;

namespace Repositories.EFCore
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
        where T : class 
    {
        protected readonly RepositoryContext _context; // DI

        public RepositoryBase(RepositoryContext context) // DI
        {
            _context = context;
        }

        public void Create(T entity) => _context.Set<T>().Add(entity);
        

        public void Delete(T entity) => _context.Set<T>().Remove(entity);
        

        public IQueryable<T> FindAll(bool trackChanges) => 
            !trackChanges ? 
            _context.Set<T>() : 
            _context.Set<T>().AsNoTracking();

        //public IQueryable<T> FindAllStocks(bool trackChanges) =>
        //    !trackChanges ?
        //    _context.Set<T>() :
        //    _context.Set<T>().AsNoTracking();



        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expresion, bool trackChanges) => 
            !trackChanges ?
            _context.Set<T>().AsNoTracking():
            _context.Set<T>().Where(expresion);

        public void Update(T entity) => _context.Set<T>().Update(entity); 
        
    }
}
