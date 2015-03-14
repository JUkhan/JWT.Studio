using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwtex.DummyData
{
    public class DDObject
    {
        public DDObject()
        {
            limit = 5;
            columns = new List<DDColumn>();
        }
        public int limit { get; set; }

        public List<DDColumn> columns { get; set; }
    }
}
