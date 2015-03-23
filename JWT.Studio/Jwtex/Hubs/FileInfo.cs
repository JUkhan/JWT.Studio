using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwtex.Hubs
{
    public class FileInfo
    {
        public string Category { get; set; }
        public string Name { get; set; }

        public string Folder { get; set; }
        public bool Lock { get; set; }
        public string UserName { get; set; }
        public string CID { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
