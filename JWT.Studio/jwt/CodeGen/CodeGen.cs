using Jwt.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwt.CodeGen
{
    public class CodeGen
    {
        protected const string TAB1 = "\t";
        protected const string TAB2 = "\t\t";
        protected const string TAB3 = "\t\t\t";
        protected const string TAB4 = "\t\t\t\t";
        protected const string TAB5 = "\t\t\t\t\t";
        protected StringBuilder _res = new StringBuilder();
        protected List<JPropertyInfo> _propList = null;
    }
    public interface ICode
    {
        string CodeGenerate(string entity, List<JPropertyInfo> props);
        JwtConfig Config { get; set; }
    }
}
