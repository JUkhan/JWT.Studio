
using EntityModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq; 
using Jwt.Dao.Service;
using Services.Interfaces;
 using Jwt.Dao.EntityFramework.Interfaces;
namespace Services.Implementation {
	public class StudentService : BaseService<Student>, IStudentService
	{

		public StudentService(IDbContextScopeFactory dbContextScopeFactory) : base(dbContextScopeFactory) { }

		public Student Insert(Student entity)
		{
			 return BaseInsert(entity);
		}
		public ICollection<Student> InsertEntities(ICollection<Student> entities)
		{
			return BaseInsertEntities(entities);
		}
		public void Update(Student entity)
		{
			BaseUpdate(entity);
		}
		public void UpdateEntities(ICollection<Student> entities)
		{
			 BaseUpdateEntities(entities);
		}
		 public Student Delete(Student entity)
		{
			return BaseDelete(entity);
		}
		public Student GetByID(dynamic id)
		{
			return BaseGetByID(id);
		}
		 public ICollection<Student> GetAll()
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
				var query = from u in context.Students
					select new {u.StudentID,u.LastName,u.FirstName,u.EnrollmentDate};
				res.Total = query.Count();
				res.Data = query.OrderBy(x=>x.StudentID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			}
			return res;
		}
		 public PagedList GetPagedWhile(int pageNo, int pageSize, Student item)
		{
			Func<Student, bool> where = s => true;
			return BaseGetPagedWhile(pageNo, pageSize, where, order => order.StudentID);
		}
	}
}