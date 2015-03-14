using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwtex.DummyData
{
    public class DDColumn
    {
        public DDColumn()
        {
            type ="human";
            min = 100;
            max = 1000;
        }
        public string name { get; set; }
        public string type { get; set; }
        public int min { get; set; }
        public int max { get; set; }
    }
}
