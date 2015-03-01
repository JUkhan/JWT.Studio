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
            AddRelationalMethods(entity);

            _res.AppendLine();   
   
            _res.Append( "}");
          
            _res.AppendLine();
            _res.AppendFormat("{0}Svc.{1}Factory.$inject=['$http'];",entity, entity[0].ToString().ToLower() + entity.Substring(1));
            _res.AppendLine();
            _res.AppendFormat("export default {0}Svc.{1}Factory;", entity, entity[0].ToString().ToLower() + entity.Substring(1));
            return _res.ToString();
        }

        private void AddRelationalMethods(string entity)
        {
            var list = _propList.Where(n => n.Details != null && n.Details.Count > 1);
           

            foreach (var item in list)
            {
                _res.AppendLine();
                _res.AppendFormat(TAB1 + "get{0}List()",item.PropertyName);
                _res.AppendLine();
                _res.Append(TAB1 + "{");

                _res.AppendLine();

                _res.Append(TAB2 + "return HTTP.get(this).get(this.root() + this.controllerName+\"/Get" + item.PropertyName + "List\");");
               
                _res.AppendLine();
                _res.Append(TAB1 + "}");
            }

            _res.AppendLine();
            _res.AppendFormat(TAB1 + "static {0}Factory(http)", entity[0].ToString().ToLower() + entity.Substring(1));
            _res.AppendLine();
            _res.Append(TAB1 + "{");

            _res.AppendLine();
            _res.AppendFormat(TAB2 + "return new {0}Svc(http);", entity);
            _res.AppendLine();
            _res.Append(TAB1 + "}");
        }
        private void AddConstructor(string entity)
        {
           
            _res.AppendLine();
            _res.AppendFormat(TAB1 + "constructor(http)");
            _res.AppendLine();
            _res.Append(TAB1 + "{");

            _res.AppendLine();
            _res.AppendFormat(TAB2 + "super('{0}',http);",entity);
            _res.AppendLine();
            _res.Append(TAB2 + "HTTP.set(this, http);");

           
            _res.AppendLine();
            _res.Append(TAB1 + "}");

        }
        private void AddUsing(string entity)
        {
            _res.Append("import BaseSvc from 'Scripts/Services/BaseSvc.js';");
            _res.AppendLine();
            _res.AppendLine();
            _res.Append("const HTTP=new WeakMap();");
            _res.AppendLine();
            _res.AppendFormat( "class {0}Svc extends BaseSvc", entity);
            _res.AppendLine();
            _res.Append( "{");
            
        }

        public JwtConfig Config
        {
            get;
            set;
        }
    }
}
