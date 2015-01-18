using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Dao.Service
{
    public class PagedList
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public dynamic Data { get; set; }

    }
}
