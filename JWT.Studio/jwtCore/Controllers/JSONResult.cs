using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Core.Controllers
{
    public class JSONResult
    {
        public bool IsSuccess { get; set; }
        public dynamic DataList { get; set; }
        public dynamic DataObject { get; set; }
        public string Message { get; set; }
        public int TotalRow { get; set; }
    }
}
