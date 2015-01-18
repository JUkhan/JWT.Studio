using Jwt.Controller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwt.CodeGen
{
    public class CSController : CodeGen, ICode
    {
        public string CodeGenerate(string entity, List<Jwt.Controller.JPropertyInfo> props)
        {
            _propList = props;
            AddUsing(entity);
            AddConstructor(entity);
            AddRelationalMethods();

            _res.AppendLine();
            _res.Append(TAB1 + "}");
            _res.AppendLine();
            _res.Append("}");
            return _res.ToString();
        }

        private void AddRelationalMethods()
        {
            var list = _propList.Where(n => n.Details!=null && n.Details.Count > 1);


            foreach (var item in list)
            {
                _res.AppendLine();
                _res.AppendFormat(TAB2 + "public JsonResult Get{0}List()", item.PropertyName);
                _res.AppendLine();
                _res.Append(TAB2 + "{");

                _res.AppendLine();
                _res.AppendFormat(TAB3 + " return Json(service.Get{0}List(), JsonRequestBehavior.AllowGet);", item.PropertyName);
                _res.AppendLine();
                _res.Append(TAB2 + "}");
            }
        }
        private void AddConstructor(string entity)
        {
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "//private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);", entity);

            _res.AppendLine();
            _res.AppendFormat(TAB2 + "private readonly I{0}Service service;", entity);
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "public {0}Controller(I{0}Service service)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");

            _res.AppendLine();
            _res.Append(TAB3 + "this.service=service;");
            _res.AppendLine();
            _res.Append(TAB3 + "SetService(service);");


            _res.AppendLine();
            _res.Append(TAB2 + "}");

        }
        private void AddUsing(string entity)
        {
            string folder = ConfigurationManager.AppSettings["EntityProject"];
            string service = ConfigurationManager.AppSettings["ServiceProject"];
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
            _res.Append("using System.Web.Mvc; ");
            _res.AppendLine();
            _res.Append("using System.Web; ");
            _res.AppendLine();
            _res.Append("//using log4net; ");
            _res.AppendLine();
            _res.Append("using System.Reflection; ");
            if (!string.IsNullOrEmpty(service))
            {
                _res.AppendLine();
                _res.AppendFormat("using {0}.Interfaces;", service);
            }
            _res.AppendLine();
            _res.Append("using Jwt.Core.Controllers; ");
            _res.AppendLine();
            _res.Append("namespace WebApp.Controllers ");
            _res.Append("{");

            string field = entity.ToLower() + "id";
            JPropertyInfo temp = _propList.FirstOrDefault(x => x.PropertyName.ToLower() == field);

            _res.AppendLine();
            _res.AppendFormat(TAB1 + "public class {0}Controller : JwtController<{0}, {1}>", entity, temp == null ? "int" : temp.Xtype);
            _res.AppendLine();
            _res.AppendLine(TAB1 + "{");

        }


        public JwtConfig Config
        {
            get;
            set;
        }
    }
}
