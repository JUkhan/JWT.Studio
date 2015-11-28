using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngEditor.Models
{
    public class Project
    {
        public Project()
        {
            startPage = "root/home";
        }
        public string name { get; set; }
        public string path { get; set; }

        public bool allowTemplate { get; set; }

        public string startPage { get; set; }
    }
    public class ProjectConfig
    {
        public ProjectConfig()
        {
            ProjectList = new List<Project>();
        }
        public List<Project> ProjectList { get; set; }
    }
}
