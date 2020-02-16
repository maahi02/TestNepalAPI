using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestNepal.Repository.Common
{
    public interface IRepository<T> where T :class
    {
        // Marks an entity as new
        void Add(T entity);
        // Marks an entity as modified
        void Update(T entity);
        // Marks an entity to be removed
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        // Get an entity by int id
        T GetById(int id, Expression<Func<T, bool>> where = null, params Expression<Func<T, object>>[] includeExpressions);
        T GetById(Int64 id, Expression<Func<T, bool>> where = null, params Expression<Func<T, object>>[] includeExpressions);

        T GetById(string id, Expression<Func<T, bool>> where = null, params Expression<Func<T, object>>[] includeExpressions);
        // Get an entity using delegate
        T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeExpressions);
        // Gets all entities of type T
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions);
        // Gets entities using delegate
        IQueryable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeExpressions);
        IQueryable<T> GetPagination(int take, int skip, Expression<Func<T, bool>> where = null, Expression<Func<T, object>> keySelector = null);

        void SaveChanges();
    }
}
