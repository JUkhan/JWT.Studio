using Jwt.Controller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwt.CodeGen
{
    public class CSServiceImplementation : CodeGen, ICode
    {
        public string CodeGenerate(string entity, List<Jwt.Controller.JPropertyInfo> props)
        {
            _propList = props;
            AddUsing(entity);
            AddMethods(entity);
            _res.AppendLine();
            _res.Append(TAB1 + "}");
            _res.AppendLine();
            _res.Append("}");
            return _res.ToString();
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
            _res.Append("using Jwt.Dao.Service;");
            if (!string.IsNullOrEmpty(service))
            {
                _res.AppendLine();
                _res.AppendFormat("using {0}.Interfaces;", service);
            }
            _res.AppendLine();
            _res.Append(" using Jwt.Dao.EntityFramework.Interfaces;");

            _res.AppendLine();
            _res.AppendFormat("namespace {0}.Implementation ", ConfigurationManager.AppSettings["ServiceProject"] ?? "Services");

            _res.Append("{");


            _res.AppendLine();
            _res.AppendFormat(TAB1 + "public class {0}Service : BaseService<{0}>, I{0}Service", entity);
            _res.AppendLine();
            _res.AppendLine(TAB1 + "{");

            _res.AppendLine();
            _res.AppendFormat(TAB2 + "public {0}Service(IDbContextScopeFactory dbContextScopeFactory) : base(dbContextScopeFactory) ", entity);
            _res.AppendLine("{ }");

        }

        private void AddMethods(string entity)
        {

            //insert
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "public {0} Insert({0} entity)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");
            _res.AppendLine();
            _res.Append(TAB3 + " return BaseInsert(entity);");
            _res.AppendLine();
            _res.Append(TAB2 + "}");
            //insert List
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "public ICollection<{0}> InsertEntities(ICollection<{0}> entities)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");
            _res.AppendLine();
            _res.Append(TAB3 + "return BaseInsertEntities(entities);");
            _res.AppendLine();
            _res.Append(TAB2 + "}");

            //update
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "public void Update({0} entity)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");
            _res.AppendLine();
            _res.Append(TAB3 + "BaseUpdate(entity);");
            _res.AppendLine();
            _res.Append(TAB2 + "}");
            //update List
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "public void UpdateEntities(ICollection<{0}> entities)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");
            _res.AppendLine();
            _res.Append(TAB3 + " BaseUpdateEntities(entities);");
            _res.AppendLine();
            _res.Append(TAB2 + "}");

            //Delete
            _res.AppendLine();
            _res.AppendFormat(TAB2 + " public {0} Delete({0} entity)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");
            _res.AppendLine();
            _res.Append(TAB3 + "return BaseDelete(entity);");
            _res.AppendLine();
            _res.Append(TAB2 + "}");

            //GetByID
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "public {0} GetByID(dynamic id)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");
            _res.AppendLine();
            _res.Append(TAB3 + "return BaseGetByID(id);");
            _res.AppendLine();
            _res.Append(TAB2 + "}");

            //GetAll
            _res.AppendLine();
            _res.AppendFormat(TAB2 + " public ICollection<{0}> GetAll()", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");
            _res.AppendLine();
            _res.Append(TAB3 + " return BaseGetAll();");
            _res.AppendLine();
            _res.Append(TAB2 + "}");

            //Count
            _res.AppendLine();
            _res.Append(TAB2 + "public int Count()");
            _res.AppendLine();
            _res.Append(TAB2 + "{");
            _res.AppendLine();
            _res.Append(TAB3 + "return BaseCount();");
            _res.AppendLine();
            _res.Append(TAB2 + "}");

            //DBContext
            string dbContext = ConfigurationManager.AppSettings["DBContext"];
            if (string.IsNullOrEmpty(dbContext))
            {
                dbContext = "Set DBContext value in web.comfig";
            }
            _res.AppendLine();
            _res.Append(TAB2 + "protected override System.Data.Entity.DbContext GetDbContext(Jwt.Dao.EntityFramework.Interfaces.IDbContextCollection dbContextCollection)");
            _res.AppendLine();
            _res.Append(TAB2 + "{");
            _res.AppendLine();
            _res.AppendFormat(TAB3 + "return dbContextCollection.Get<{0}>();", dbContext);
            _res.AppendLine();
            _res.Append(TAB2 + "}");

            //GetPagedList
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "public PagedList GetPaged(int pageNo, int pageSize)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");

            _res.AppendLine();
            _res.Append(TAB3 + "PagedList res = new PagedList();");
            _res.AppendLine();
            _res.Append(TAB3 + "using (var dbContextScope = _dbContextScopeFactory.Create())");
            _res.AppendLine();
            _res.Append(TAB3 + "{");

            _res.AppendLine();
            _res.AppendFormat(TAB4 + "{0} context = ({0})GetDbContext(dbContextScope.DbContexts);", dbContext);

            _res.AppendLine();
            _res.AppendFormat(TAB4 + "var query = from u in context.{0}s", entity);
            _res.AppendLine();
            _res.Append(TAB5 + "select new {");
            _res.AppendFormat("u.{0}ID", entity);
            foreach (var item in _propList)
            {
                if (item.Checked)
                {
                    _res.AppendFormat(",u.{0}", item.PropertyName);
                }
                if (item.Details != null && item.Details.Count > 0)
                {
                    foreach (var item2 in item.Details)
                    {
                        if (item2.Checked && !(item2.PropertyName.ToLower().Contains("id")))
                        {
                            _res.AppendFormat(",{0}_{1}=u.{0}.{1}", item.PropertyName, item2.PropertyName);
                        }
                    }
                }
            }
            _res.Append("};");
            _res.AppendLine();
            _res.Append(TAB4 + "res.Total = query.Count();");

            _res.AppendLine();
            _res.AppendFormat(TAB4 + "res.Data = query.OrderBy(x=>x.{0}ID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();", entity);

            _res.AppendLine();
            _res.Append(TAB3 + "}");

            _res.AppendLine();
            _res.Append(TAB3 + "return res;");

            _res.AppendLine();
            _res.Append(TAB2 + "}");

            //GetPagedWhile
            _res.AppendLine();
            _res.AppendFormat(TAB2 + " public PagedList GetPagedWhile(int pageNo, int pageSize, {0} item)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");
            _res.AppendLine();
            _res.AppendFormat(TAB3 + "Func<{0}, bool> where = s => true;", entity);
            _res.AppendLine();
            _res.AppendFormat(TAB3 + "return BaseGetPagedWhile(pageNo, pageSize, where, order => order.{0}ID);", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "}");

            //////////////////////////
            var list = _propList.Where(n => n.Details!=null && n.Details.Count > 1);
            foreach (var item in list)
            {
                _res.AppendLine();
                _res.AppendFormat(TAB2 + "public PagedList Get{0}List()", item.PropertyName);
                _res.AppendLine();
                _res.Append(TAB2 + "{");

                _res.AppendLine();
                _res.Append(TAB3 + "PagedList pagedList=new PagedList();");


                _res.AppendLine();
                _res.Append(TAB3 + "using (var dbContextScope = _dbContextScopeFactory.Create())");
                _res.AppendLine();
                _res.Append(TAB3 + "{");

                _res.AppendLine();
                _res.AppendFormat(TAB4 + "{0} context = ({0})GetDbContext(dbContextScope.DbContexts);", dbContext);

                _res.AppendLine();
                _res.AppendFormat(TAB4 + "var query = from u in context.{0}s", item.Xtype.Substring(item.Xtype.LastIndexOf('.')+1));
                _res.AppendLine();
                _res.Append(TAB5 + "select new {");

                string comma = "";
                if(item.Details!=null)
                foreach (var xitem in item.Details)
                {
                    if (xitem.Checked)
                    {

                        _res.AppendFormat("{0}{1}=u.{1}", comma, xitem.PropertyName);
                        comma = ",";

                    }
                }
                _res.Append("};");

                _res.AppendLine();
                _res.Append(TAB4 + "pagedList.Data=query.ToList();");

                _res.AppendLine();
                _res.Append(TAB3 + "}");

                _res.AppendLine();
                _res.Append(TAB3 + "return pagedList;");

                _res.AppendLine();
                _res.Append(TAB2 + "}");

            }

        }

        public JwtConfig Config
        {
            get;
            set;
        }
    }
}
