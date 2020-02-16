
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using TestNepal.Context;

namespace TestNepal.Repository.Common
{
    public abstract class RepositoryBase<T> where T : class
    {
        #region Properties
        private TestNepalContext dataContext;
        private readonly DbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected TestNepalContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        #endregion
        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }
        #region Implementation
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            //dbSet.Attach(entity);
            //dataContext.Entry(entity).State = EntityState.Modified;
            dataContext.Set<T>().AddOrUpdate(entity);
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (var obj in objects)
            {
                dbSet.Remove(obj);
            }
        }
        public virtual T GetById(int id, Expression<Func<T, bool>> where = null, params Expression<Func<T, object>>[] includeExpressions)
        {

            if (includeExpressions.Any())
            {
                var set = includeExpressions
                  .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                    (dbSet, (current, expression) => current.Include(expression));

                return set.FirstOrDefault(where);
            }
            if (id == 0)
            {
                return dbSet.FirstOrDefault(where);
            }

            return dbSet.Find(id);
        }
        public virtual T GetById(Int64 id, Expression<Func<T, bool>> where = null, params Expression<Func<T, object>>[] includeExpressions)
        {

            if (includeExpressions.Any())
            {
                var set = includeExpressions
                  .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                    (dbSet, (current, expression) => current.Include(expression));

                return set.FirstOrDefault(where);
            }
            if (id == 0)
            {
                return dbSet.FirstOrDefault(where);
            }

            return dbSet.Find(id);
        }
        
        public virtual T GetById(String id, Expression<Func<T, bool>> where = null, params Expression<Func<T, object>>[] includeExpressions)
        {

            if (includeExpressions.Any())
            {
                var set = includeExpressions
                  .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                    (dbSet, (current, expression) => current.Include(expression));

                return set.FirstOrDefault(where);
            }
            if (String.IsNullOrEmpty(id))
            {
                return dbSet.FirstOrDefault(where);
            }

            return dbSet.Find(id);
        }
        public T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> set = dbSet;
            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }
            return set.Where(where).FirstOrDefault<T>();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> set = dbSet;
            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }
            return set;
        }
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> set = dbSet;
            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }
            return set.Where(where);
        }

        public virtual IQueryable<T> GetPagination(int take, int skip, Expression<Func<T, bool>> where = null, Expression<Func<T, object>> keySelector = null)
        {
            IQueryable<T> set = dbSet;
            if (where != null)
            {
                set = dbSet.Where(where);
            }
            if (keySelector != null)
            {
                var a = set.OrderBy(keySelector).Skip(skip).Take(take).ToList();
                return set.OrderBy(keySelector).Skip(skip).Take(take);
            }
            return set.Skip(skip).Take(take);
        }

        public void SaveChanges()
        {
            this.DbContext.SaveChanges();
        }
        #endregion
    }
}
