using Jwt.Dao.EntityFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Dao.Service
{
    public abstract class BaseService<T> where T : class
    {
        protected readonly IDbContextScopeFactory _dbContextScopeFactory;

        public BaseService(IDbContextScopeFactory dbContextScopeFactory)
        {
            _dbContextScopeFactory = dbContextScopeFactory;

        }
        protected abstract DbContext GetDbContext(IDbContextCollection dbContextCollection);
        private DbSet<T> GetDbSet(DbContext context)
        {
           
            return context.Set<T>();
            
        }
        public virtual ICollection<T> BaseGetAll()
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var context = GetDbContext(dbContextScope.DbContexts);                
                return context.Set<T>().ToList<T>();
            }
        }

        public virtual PagedList BaseGetPaged(int pageNo, int pageSize, Func<T, Object> orderBy)
        {
            PagedList res = new PagedList();
            res.PageNo = pageNo;
            res.PageSize = pageSize;
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var context = GetDbContext(dbContextScope.DbContexts);
                var data = GetDbSet(context).AsQueryable<T>();
                res.Total = data.Count();
                res.Data = data.OrderBy(orderBy).Skip((pageNo - 1)*pageSize).Take(pageSize).ToList();
            }
            return res;

        }
        public virtual PagedList BaseGetPagedWhile(int pageNo, int pageSize, Func<T, bool> wherePredicate, Func<T, Object> orderBy)
        {
            PagedList res = new PagedList();
            res.PageNo = pageNo;
            res.PageSize = pageSize;

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var context = GetDbContext(dbContextScope.DbContexts);
                var data = GetDbSet(context).AsQueryable<T>().Where(wherePredicate);
                res.Total = data.Count();
                res.Data = data.OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            return res;           
        }
        public virtual T BaseGetByID(dynamic Id)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var context = GetDbContext(dbContextScope.DbContexts);
                dynamic dbResult = this.GetDbSet(context).Find(Id);
                return dbResult;
            }
        }
        public virtual T BaseInsert(T entity)
        {
            T obj = default(T);
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var context = GetDbContext(dbContextScope.DbContexts);
                obj =this.GetDbSet(context).Add(entity);
                dbContextScope.SaveChanges();
            }
            return obj;
        }
        public ICollection<T> BaseInsertEntities(ICollection<T> entities)
        {
            ICollection<T> list = new List<T>();
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                foreach (var item in entities)
                {
                    list.Add(BaseInsert(item));
                }
                dbContextScope.SaveChanges();
            }
            return list;
        }
        public virtual void BaseUpdate(dynamic entity)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var context = GetDbContext(dbContextScope.DbContexts);

                if (context.Entry(entity).State == EntityState.Detached)
                {
                    GetDbSet(context).Attach(entity);
                }
                context.Entry(entity).State = EntityState.Modified;
                dbContextScope.SaveChanges();
            }

        }
        public void BaseUpdateEntities(ICollection<T> entities)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                foreach (var item in entities)
                {
                    BaseUpdate(item);
                }
                dbContextScope.SaveChanges();
            }
        }
        public virtual T BaseDelete(dynamic entity)
        {
            T obj = default(T);
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var context = GetDbContext(dbContextScope.DbContexts);
                var dbSet = GetDbSet(context);
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
                obj = (dynamic)dbSet.Remove(entity);

                dbContextScope.SaveChanges();
            }
            return obj;
        }

        public virtual ObjectContext ObjectContext
        {
            get
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    return ((IObjectContextAdapter)this.GetDbContext(dbContextScope.DbContexts)).ObjectContext;
                }
            }
        }
        public virtual int BaseCount()
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var sql = "Select count(*) from " + typeof(T).Name;
                return this.ObjectContext.ExecuteStoreQuery<int>(sql).FirstOrDefault();
            }

        }
    }
}
