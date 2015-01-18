using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Dao.Service
{
    public interface IBaseService<T>
    {
        T Insert(T entity);
        ICollection<T> InsertEntities(ICollection<T> entities);
        T GetByID(dynamic id);
        ICollection<T> GetAll();
        PagedList GetPaged(int pageNo, int pageSize);
        PagedList GetPagedWhile(int pageNo, int pageSize, T item);
        void Update(T entity);
        void UpdateEntities(ICollection<T> entities);
        T Delete(T entity);
        int Count();
        ObjectContext ObjectContext { get; }
    }
}
