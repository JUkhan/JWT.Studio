
using Jac.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq; 
using Jwt.Dao.Service;
using Services.Interfaces;
 using Jwt.Dao.EntityFramework.Interfaces;
namespace Services.Implementation {
	public class InstructorService : BaseService<Instructor>, IInstructorService
	{

		public InstructorService(IDbContextScopeFactory dbContextScopeFactory) : base(dbContextScopeFactory) { }

		public Instructor Insert(Instructor entity)
		{
			 return BaseInsert(entity);
		}
		public ICollection<Instructor> InsertEntities(ICollection<Instructor> entities)
		{
			return BaseInsertEntities(entities);
		}
		public void Update(Instructor entity)
		{
			BaseUpdate(entity);
		}
		public void UpdateEntities(ICollection<Instructor> entities)
		{
			 BaseUpdateEntities(entities);
		}
		 public Instructor Delete(Instructor entity)
		{
			return BaseDelete(entity);
		}
		public Instructor GetByID(dynamic id)
		{
			return BaseGetByID(id);
		}
		 public ICollection<Instructor> GetAll()
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
				var query = from u in context.Instructors
					select new {u.InstructorID,u.LastName,u.FirstName,u.HireDate,OfficeAssignment_Location=u.OfficeAssignment.Location};
				res.Total = query.Count();
				res.Data = query.OrderBy(x=>x.InstructorID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			}
			return res;
		}
		 public PagedList GetPagedWhile(int pageNo, int pageSize, Instructor item)
		{
			Func<Instructor, bool> where = s => true;
			return BaseGetPagedWhile(pageNo, pageSize, where, order => order.InstructorID);
		}
		public PagedList GetOfficeAssignmentList()
		{
			PagedList pagedList=new PagedList();
			using (var dbContextScope = _dbContextScopeFactory.Create())
			{
				SchoolContext context = (SchoolContext)GetDbContext(dbContextScope.DbContexts);
				var query = from u in context.OfficeAssignments
					select new {InstructorID=u.InstructorID,Location=u.Location};
				pagedList.Data=query.ToList();
			}
			return pagedList;
		}
	}
}