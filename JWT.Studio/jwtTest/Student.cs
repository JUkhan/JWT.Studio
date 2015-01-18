using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jwtApp.jwtTest
{
    public class Student
    {
        
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
       
        public DateTime EnrollmentDate { get; set; }

       
    }
}