using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jac.Entities.Entities
{
    public class Department
    {
        public int DepartmentID { get; set; }
       
        public string Name { get; set; }
       
        public decimal Budget { get; set; }
        
        public DateTime StartDate { get; set; }
        public int InstructorID { get; set; }
        public virtual Instructor Administrator { get; set; }   
        public virtual ICollection<Course> Courses { get; set; }
    }
}
