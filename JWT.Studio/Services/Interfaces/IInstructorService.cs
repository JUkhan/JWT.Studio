
using Jac.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq; 
using Jwt.Dao.Service;
namespace Services.Interfaces {
	public interface IInstructorService : IBaseService<Instructor>
	{

		PagedList GetOfficeAssignmentList();
	}
}