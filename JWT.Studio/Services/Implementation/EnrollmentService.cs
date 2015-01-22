
using EntityModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq; 
using Jwt.Dao.Service;
using Services.Interfaces;
 using Jwt.Dao.EntityFramework.Interfaces;
namespace Services.Implementation {
	public class EnrollmentService : BaseService<Enrollment>, IEnrollmentService
	{

		public EnrollmentService(IDbContextScopeFactory dbContextScopeFactory) : base(dbContextScopeFactory) { }

		public Enrollment Insert(Enrollment entity)
		{
			 return BaseInsert(entity);
		}
		public ICollection<Enrollment> InsertEntities(ICollection<Enrollment> entities)
		{
			return BaseInsertEntities(entities);
		}
		public void Update(Enrollment entity)
		{
			BaseUpdate(entity);
		}
		public void UpdateEntities(ICollection<Enrollment> entities)
		{
			 BaseUpdateEntities(entities);
		}
		 public Enrollment Delete(Enrollment entity)
		{
			return BaseDelete(entity);
		}
		public Enrollment GetByID(dynamic id)
		{
			return BaseGetByID(id);
		}
		 public ICollection<Enrollment> GetAll()
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
				var query = from u in context.Enrollments
					select new {u.EnrollmentID,u.CourseID,u.StudentID,u.Grade,Course_Title=u.Course.Title,Student_FirstName=u.Student.FirstName};
				res.Total = query.Count();
				res.Data = query.OrderBy(x=>x.EnrollmentID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			}
			return res;
		}
		 public PagedList GetPagedWhile(int pageNo, int pageSize, Enrollment item)
		{
			Func<Enrollment, bool> where = s => true;
			return BaseGetPagedWhile(pageNo, pageSize, where, order => order.EnrollmentID);
		}
		public PagedList GetCourseList()
		{
			PagedList pagedList=new PagedList();
			using (var dbContextScope = _dbContextScopeFactory.Create())
			{
				SchoolContext context = (SchoolContext)GetDbContext(dbContextScope.DbContexts);
				var query = from u in context.Courses
					select new {CourseID=u.CourseID,Title=u.Title};
				pagedList.Data=query.ToList();
			}
			return pagedList;
		}
		public PagedList GetStudentList()
		{
			PagedList pagedList=new PagedList();
			using (var dbContextScope = _dbContextScopeFactory.Create())
			{
				SchoolContext context = (SchoolContext)GetDbContext(dbContextScope.DbContexts);
				var query = from u in context.Students
					select new {StudentID=u.StudentID,FirstName=u.FirstName};
				pagedList.Data=query.ToList();
			}
			return pagedList;
		}
	}
}