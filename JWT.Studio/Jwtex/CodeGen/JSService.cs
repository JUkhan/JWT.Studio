using Jwt.Controller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwt.CodeGen
{
    public class JSService:CodeGen, ICode
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
            var list = _propList.Where(n => n.Details != null && n.Details.Count > 1);
           

            foreach (var item in list)
            {
                _res.AppendLine();
                _res.AppendFormat(TAB2 + "public Promise Get{0}List()",item.PropertyName);
                _res.AppendLine();
                _res.Append(TAB2 + "{");

                _res.AppendLine();
                _res.Append(TAB3 + "Deferred deffer = this.qService.defer();");

                _res.AppendLine();
                _res.Append(TAB3 + " this.http.get(\"{0}/Get"+item.PropertyName+"List\".format(this.Root() + this.controllerName))");
                _res.AppendLine();
                _res.Append(TAB3 + ".success((res, status) => { deffer.resolve(res); })");
                _res.AppendLine();
                _res.Append(TAB3 + ".error((res, status) => { deffer.reject(res); });");

                _res.AppendLine();
                _res.Append(TAB3+"return deffer.promise;");
                _res.AppendLine();
                _res.Append(TAB2 + "}");
            }
        }
        private void AddConstructor(string entity)
        {
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "private HttpService http = null;", entity);

            _res.AppendLine();
            _res.AppendFormat(TAB2 + "private QService qService = null;", entity);
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "public {0}Service(HttpService http, QService qService):base(\"{0}\", http, qService)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");

            _res.AppendLine();
            _res.Append(TAB3 + "this.http=http;");
            _res.AppendLine();
            _res.Append(TAB3 + "this.qService=qService;");

           
            _res.AppendLine();
            _res.Append(TAB2 + "}");

        }
        private void AddUsing(string entity)
        {
            _res.AppendFormat(@"//opath={0}Scripts\Services\{1}Service.js,ab=true", Config.Root, entity);
            string folder = ConfigurationManager.AppSettings["EntityProject"];
            if (!string.IsNullOrEmpty(folder))
            {
                _res.AppendLine();
                _res.AppendFormat("using {0}.Entities;", folder);
            }
            _res.AppendLine();
            _res.Append("using CSharp.Wrapper.Angular;");
            _res.AppendLine();
            _res.Append("using CSharp.Wrapper.JS;");
            _res.AppendLine();
            _res.Append("using Scripts.Services;");
            _res.AppendLine();
            _res.Append("using System;");
            _res.AppendLine();
            _res.Append("using System.Collections.Generic;");
            _res.AppendLine();
            _res.Append("using System.Linq;");
            _res.AppendLine();
            _res.Append("namespace Scripts.Services");
            _res.AppendLine();
            _res.Append("{");
            
            _res.AppendLine();
            _res.AppendFormat(TAB1 + "[Angular(ModuleName = \"app\", ActionType = \"factory\", ActionName = \"{0}Service\", DI = \"$http,$q\")]", entity);
            _res.AppendLine();
            _res.AppendFormat(TAB1 + "public class {0}Service : BaseService<{0}>", entity);
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
