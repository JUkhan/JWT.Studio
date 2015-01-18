using Jwt.Dao.EntityFramework.Implementation;
using Jwt.Dao.EntityFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jwt.Dao.Repository;
using System.Data.Entity.Core.Objects;

namespace Jwt.Dao.Service
{
    public class BaseService2<T>
    {
        protected readonly IDbContextScopeFactory _dbContextScopeFactory;
        protected CrudRepository<T> _repository;
        public BaseService2(CrudRepository<T> repository)
        {
            _dbContextScopeFactory = new DbContextScopeFactory();
            _repository = repository; //DuckTyping.Cast<ICrudRepository>(repository);
        }
        public virtual ICollection<T> GetAll()
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                dynamic res = _repository.GetAll().Cast<T>().ToList();
                return res;
            }
        }
        public virtual PagedList GetPaged(int pageNo, int pageSize, Func<T, Object> orderBy)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                return _repository.GetPaged(pageNo, pageSize, orderBy);
            }
        }
        public virtual PagedList GetPagedWhile(int pageNo, int pageSize, Func<T, bool> predicate, Func<T, Object> orderBy)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                return _repository.GetPagedWhile(pageNo, pageSize, predicate,  orderBy);
            }
        }
         
        public virtual T GetByID(dynamic Id)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                return _repository.GetByID(Id);
            }
        }
        public virtual T Insert(T entity)
        {
            T obj = default(T);
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                obj = _repository.Insert(entity);
                dbContextScope.SaveChanges();
            }
            return obj;
        }
        public ICollection<T> InsertEntities(ICollection<T> entities)
        {
            ICollection<T> list = new List<T>();
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                foreach (var item in entities)
                {
                    list.Add(Insert(item));
                }
                dbContextScope.SaveChanges();
            }
            return list;
        }
        public virtual void Update(T entity)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                _repository.Update(entity);
                dbContextScope.SaveChanges();
            }

        }
        public void UpdateEntities(ICollection<T> entities)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                foreach (var item in entities)
                {
                    Update(item);
                }
                dbContextScope.SaveChanges();
            }
        }
        public virtual T Delete(T entity)
        {
            T obj = default(T);
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                obj = _repository.Delete(entity);
                dbContextScope.SaveChanges();
            }
            return obj;
        }

        public ObjectContext ObjectContext
        {
            get
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    return _repository.ObjectContext;
                }
            }
        }
        public int Count()
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                return _repository.Count();
            }

        }

    }
}
