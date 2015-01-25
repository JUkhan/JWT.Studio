using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwtex
{
    public class JResult
    {
        public bool isSuccess { get; set; }
        public dynamic data { get; set; }
        public string msg { get; set; }
    }
}
