using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jac.Entities.Entities
{
    public class OfficeAssignment
    {
       
        public int InstructorID { get; set; }
        
        public string Location { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
