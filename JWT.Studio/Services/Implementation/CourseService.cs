
using Jac.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq; 
using Jwt.Dao.Service;
using Services.Interfaces;
 using Jwt.Dao.EntityFramework.Interfaces;
namespace Services.Implementation {
	public class CourseService : BaseService<Course>, ICourseService
	{

		public CourseService(IDbContextScopeFactory dbContextScopeFactory) : base(dbContextScopeFactory) { }

		public Course Insert(Course entity)
		{
			 return BaseInsert(entity);
		}
		public ICollection<Course> InsertEntities(ICollection<Course> entities)
		{
			return BaseInsertEntities(entities);
		}
		public void Update(Course entity)
		{
			BaseUpdate(entity);
		}
		public void UpdateEntities(ICollection<Course> entities)
		{
			 BaseUpdateEntities(entities);
		}
		 public Course Delete(Course entity)
		{
			return BaseDelete(entity);
		}
		public Course GetByID(dynamic id)
		{
			return BaseGetByID(id);
		}
		 public ICollection<Course> GetAll()
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
				var query = from u in context.Courses
					select new {u.CourseID,u.Title,u.Credits,u.DepartmentID,Department_Name=u.Department.Name};
				res.Total = query.Count();
				res.Data = query.OrderBy(x=>x.CourseID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			}
			return res;
		}
		 public PagedList GetPagedWhile(int pageNo, int pageSize, Course item)
		{
			Func<Course, bool> where = s => true;
			return BaseGetPagedWhile(pageNo, pageSize, where, order => order.CourseID);
		}
		public PagedList GetDepartmentList()
		{
			PagedList pagedList=new PagedList();
			using (var dbContextScope = _dbContextScopeFactory.Create())
			{
				SchoolContext context = (SchoolContext)GetDbContext(dbContextScope.DbContexts);
				var query = from u in context.Departments
					select new {DepartmentID=u.DepartmentID,Name=u.Name};
				pagedList.Data=query.ToList();
			}
			return pagedList;
		}
	}
}