using Jwt.Controller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwt.CodeGen
{
    public class CSServiceInterface: CodeGen,ICode
    {
        public string CodeGenerate(string entity, List<Jwt.Controller.JPropertyInfo> props)
        {
            _propList = props;
            AddUsing(entity);
            _res.AppendLine();
            _res.Append(TAB1 + "}");
            _res.AppendLine();
            _res.Append("}");
            return _res.ToString();
        }
        private void AddUsing(string entity)
        {
            string folder = ConfigurationManager.AppSettings["EntityProject"];
            if (!string.IsNullOrEmpty(folder))
            {
                _res.AppendLine();
                _res.AppendFormat("using {0}.Entities;", folder);
            }
            _res.AppendLine();
            _res.Append("using System;");
            _res.AppendLine();
            _res.Append("using System.Collections.Generic;");
            _res.AppendLine();
            _res.Append("using System.Linq; ");
            _res.AppendLine();
            _res.Append("using Jwt.Dao.Service;");
           
            _res.AppendLine();
            _res.AppendFormat("namespace {0}.Interfaces ", ConfigurationManager.AppSettings["ServiceProject"]??"Services");
            _res.Append("{");

          
            _res.AppendLine();
            _res.AppendFormat(TAB1 + "public interface I{0}Service : IBaseService<{0}>", entity);
            _res.AppendLine();
            _res.AppendLine(TAB1 + "{");

            var list = _propList.Where(n => n.Details!=null && n.Details.Count > 1);


            foreach (var item in list)
            {
                _res.AppendLine();
                _res.AppendFormat(TAB2 + "PagedList Get{0}List();", item.PropertyName);
               
            }

        }

        public JwtConfig Config
        {
            get;
            set;
        }
    }
}
