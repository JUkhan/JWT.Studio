using Jwt.Dao.EntityFramework.Implementation;
using Jwt.Dao.EntityFramework.Interfaces;
using Jwt.Dao.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Dao.Repository
{
    public abstract class CrudRepository<T> where T : class
    {
        protected readonly IAmbientDbContextLocator _ambientDbContextLocator;

        public CrudRepository()
        {
            this._ambientDbContextLocator = new AmbientDbContextLocator();

        }
        private DbSet<T> GetDbSet()
        {
            return this.DbContext.Set<T>();
        }
        protected abstract DbContext DbContext
        {
            get;
        }
        public virtual T Insert(T entity)
        {
            dynamic obj = this.GetDbSet().Add(entity);

            return obj;
        }

        public virtual T GetByID(dynamic id)
        {
           return  this.GetDbSet().Find(id);          
        }

        public virtual ICollection<T> GetAll()
        {
            return this.GetDbSet().ToList();

        }
        public virtual PagedList GetPagedList(int pageNo, int pageSize, Func<T, Object> orderBy)
        {
            PagedList res = new PagedList();
            res.PageNo = pageNo;
            res.PageSize = pageSize;

            var data = this.GetDbSet().AsQueryable<T>();
            res.Total = data.Count();
            res.Data = data.OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return res;
        }
        public virtual PagedList GetPagedListWhile(int pageNo, int pageSize, Func<T, bool> predicate, Func<T, Object> orderBy)
        {
            PagedList res = new PagedList();
            res.PageNo = pageNo;
            res.PageSize = pageSize;

            var data = this.GetDbSet().AsQueryable<T>().Where(predicate);
            res.Total = data.Count();
            res.Data = data.OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return res;

        }
        public virtual void Update(dynamic entity)
        {
            if (this.DbContext.Entry(entity).State == EntityState.Detached)
            {
                GetDbSet().Attach(entity);
            }
            this.DbContext.Entry(entity).State = EntityState.Modified;

        }

        public virtual T Delete(dynamic entity)
        {
            var dbSet = GetDbSet();
            if (this.DbContext.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dynamic obj = dbSet.Remove(entity);
            return obj;
        }

        public virtual ObjectContext ObjectContext
        {
            get {
                return ((IObjectContextAdapter)this.DbContext).ObjectContext;
            }
        }
        public virtual int Count()
        {
            var sql = "Select count(*) from " + typeof(T).Name;
            return this.ObjectContext.ExecuteStoreQuery<int>(sql).FirstOrDefault();

        }

    }
}
