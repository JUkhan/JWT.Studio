
using Jac.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq; 
using Jwt.Dao.Service;
using Services.Interfaces;
 using Jwt.Dao.EntityFramework.Interfaces;
namespace Services.Implementation {
	public class DepartmentService : BaseService<Department>, IDepartmentService
	{

		public DepartmentService(IDbContextScopeFactory dbContextScopeFactory) : base(dbContextScopeFactory) { }

		public Department Insert(Department entity)
		{
			 return BaseInsert(entity);
		}
		public ICollection<Department> InsertEntities(ICollection<Department> entities)
		{
			return BaseInsertEntities(entities);
		}
		public void Update(Department entity)
		{
			BaseUpdate(entity);
		}
		public void UpdateEntities(ICollection<Department> entities)
		{
			 BaseUpdateEntities(entities);
		}
		 public Department Delete(Department entity)
		{
			return BaseDelete(entity);
		}
		public Department GetByID(dynamic id)
		{
			return BaseGetByID(id);
		}
		 public ICollection<Department> GetAll()
		{
			 return BaseGetAll();
		}
		public int Count()
		{
			return BaseCount();
		}
		protected override System.Data.Entity.DbContext GetDbContext(Jwt.Dao.EntityFramework.Interfaces.IDbContextCollection dbContextCollection)
		{
			return dbContextCollection.Get<SchoolContext>();
		}
		public PagedList GetPaged(int pageNo, int pageSize)
		{
			PagedList res = new PagedList();
			using (var dbContextScope = _dbContextScopeFactory.Create())
			{
				SchoolContext context = (SchoolContext)GetDbContext(dbContextScope.DbContexts);
				var query = from u in context.Departments
					select new {u.DepartmentID,u.Name,u.Budget,u.StartDate,u.InstructorID,Administrator_FirstName=u.Administrator.FirstName};
				res.Total = query.Count();
				res.Data = query.OrderBy(x=>x.DepartmentID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			}
			return res;
		}
		 public PagedList GetPagedWhile(int pageNo, int pageSize, Department item)
		{
			Func<Department, bool> where = s => true;
			return BaseGetPagedWhile(pageNo, pageSize, where, order => order.DepartmentID);
		}
		public PagedList GetAdministratorList()
		{
			PagedList pagedList=new PagedList();
			using (var dbContextScope = _dbContextScopeFactory.Create())
			{
				SchoolContext context = (SchoolContext)GetDbContext(dbContextScope.DbContexts);
				var query = from u in context.Instructors
					select new {InstructorID=u.InstructorID,FirstName=u.FirstName};
				pagedList.Data=query.ToList();
			}
			return pagedList;
		}
	}
}