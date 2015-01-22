
using EntityModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq; 
using Jwt.Dao.Service;
namespace Services.Interfaces {
	public interface IEnrollmentService : IBaseService<Enrollment>
	{

		PagedList GetCourseList();
		PagedList GetStudentList();
	}
}