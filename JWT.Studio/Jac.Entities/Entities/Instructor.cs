using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jac.Entities.Entities
{
    public class Instructor
    {
        public int InstructorID { get; set; }
       
        public string LastName { get; set; }
       
        public string FirstName { get; set; }       
        public DateTime HireDate { get; set; }
       
        public virtual ICollection<Course> Courses { get; set; } 
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}
