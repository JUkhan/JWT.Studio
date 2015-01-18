using Jwt.Controller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwt.CodeGen
{
    public class JSController : CodeGen, ICode
    {
        public string CodeGenerate(string entity, List<Jwt.Controller.JPropertyInfo> props)
        {
            _propList = props;
            AddUsing(entity);
            AddConstructor(entity);
            AddRemoveMethod(entity);
            _res.AppendLine();
            _res.Append(TAB1 + "}");
            _res.AppendLine();
            _res.Append("}");
            return _res.ToString();
        }
        private bool hasDatetime = false;
        private StringBuilder scopeInitialized = new StringBuilder();
        private StringBuilder dateResolved = new StringBuilder();
        private void AddConstructor(string entity)
        {
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "private {0}Scope scope = null;", entity);

            _res.AppendLine();
            _res.AppendFormat(TAB2 + "private {0}Service service = null;", entity);
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "public {0}Ctrl({0}Scope scope, {0}Service service, Sce sce):base(scope, service, sce)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");

            _res.AppendLine();
            _res.Append(TAB3 + "this.scope=scope;");
            _res.AppendLine();
            _res.Append(TAB3 + "this.service=service;");
            _res.Append(this.scopeInitialized.ToString());

            _res.AppendLine();
            _res.Append(TAB3 + "scope.gridOpts.columnDefs = new List<ColumnDef>{");

            _res.AppendLine();
            _res.Append(TAB4 + "new ColumnDef{ name=\"AC\",  width=50, enableSorting=false, cellTemplate=\"<div style='text-align:center'><a ng-click=\\\"getExternalScopes().EditAction(row)\\\" href=\\\"javascript:;\\\"> <i class=\\\"fa fa-pencil\\\"></i>  </a><a ng-click=\\\"getExternalScopes().RemoveAction(row)\\\" href=\\\"javascript:;\\\"> <i class=\\\"fa fa-trash\\\"></i>  </a></div>\"}");
            foreach (var item in _propList)
            {
                if (item.Checked)
                {

                    if (item.UiType == "date")
                    {
                        _res.AppendLine();
                        _res.Append(TAB4 + ",new ColumnDef{ name=\"" + item.PropertyName + "\" ,cellFilter=\"date:'yyyy-MM-dd'\"}");
                    }
                    else if (item.UiType == "select")
                    {
                        string field = item.PropertyName.ToLower().Replace("id", "");
                        JPropertyInfo temp = _propList.FirstOrDefault(x => x.PropertyName.ToLower() == field);
                        if (temp == null)
                        {
                         temp= _propList.FirstOrDefault(x => x.Xtype.ToLower().Contains(field));
                        }
                        if (temp != null && temp.Details != null && temp.Details.Count > 0)
                        {
                            var x=temp.Details.FirstOrDefault(m=>m.Checked&&!(m.PropertyName.ToLower().Contains("id")));
                            if(x!=null){
                                 _res.AppendLine();
                                _res.Append(TAB4 + ",new ColumnDef{ name=\""+temp.PropertyName+"\", field=\""+temp.PropertyName+"_"+x.PropertyName +"\"}");
                            }
                        }
                    }
                    else
                    {                        
                        _res.AppendLine();
                        _res.Append(TAB4 + ",new ColumnDef{ name=\""+item.PropertyName+"\"}");
                    }
                }
            }
            _res.AppendLine();
            _res.Append(TAB3 + "};");
            _res.AppendLine();
            _res.Append(TAB3+ "scope.gridOpts.onRegisterApi = gridApi => { ");
            _res.AppendLine();
            _res.Append(TAB4 + "gridApi.paging.on.pagingChanged(scope,(newPage, pageSize)=>{");
                _res.AppendLine();
                _res.Append(TAB5 + "this.pageNo = newPage;");
                _res.AppendLine();
                _res.Append(TAB5 + "this.pageSize = pageSize;");
                _res.AppendLine();
                _res.Append(TAB5 + "this.GetPaged();");
                _res.AppendLine();
                _res.Append(TAB4 + "});");
                _res.AppendLine();
                _res.Append(TAB3 + "};");

           
            _res.AppendLine();
            _res.Append(TAB3 + "this.GetPaged();");
            _res.AppendLine();
            _res.Append(TAB3 + "this.LoadRelationalData();");
            _res.AppendLine();
            _res.Append(TAB2 + "}");

        }
        public void AddRemoveMethod(string entity)
        {
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "protected override void OnAfterDeleted({0} item)", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "{");

            _res.AppendLine();
            _res.AppendFormat(TAB3 + "this.scope.list.remove(x => x.{0}ID == item.{0}ID);", entity);
            _res.AppendLine();
            _res.Append(TAB2 + "}");

            if (hasDatetime)
            {
                _res.AppendLine();
                _res.AppendFormat(TAB2 + "protected override List<{0}> OnPreLoad(List<{0}> dataList)", entity);
                _res.AppendLine();
                _res.Append(TAB2 + "{");

                _res.AppendLine();
                _res.Append(TAB3 + "dataList.ForEach(item => {");
                _res.AppendLine();
                _res.Append(dateResolved.ToString());
                _res.AppendLine();
                _res.Append(TAB3 + " });");
                _res.AppendLine();
                _res.Append(TAB3 + "return dataList;");
                _res.AppendLine();
                _res.Append(TAB2 + "}");
                /////////////////

                _res.AppendLine();
                _res.AppendFormat(TAB2 + "protected override {0} OnBeforeAddInList({0} item)", entity);
                _res.AppendLine();
                _res.Append(TAB2 + "{");

                _res.AppendLine();
                _res.AppendFormat(dateResolved.ToString());
                _res.AppendLine();
                _res.AppendFormat(TAB3 + "return item;");
                _res.AppendLine();
                _res.Append(TAB2 + "}");
            }

            //loadRelationalData
            var list = _propList.Where(n => n.Details != null && n.Details.Count > 1);
            _res.AppendLine();
            _res.Append(TAB2 + "protected void LoadRelationalData()");
            _res.AppendLine();
            _res.Append(TAB2 + "{");

            foreach (var item in list)
            {
                _res.AppendLine();
                _res.AppendFormat(TAB3 + "this.service.Get{0}List().then(p =>", item.PropertyName);
                _res.AppendLine();
                _res.Append(TAB3+"{");
                _res.AppendLine();
                _res.AppendFormat(TAB4 + "this.scope.{0}List=p.Data;", item.PropertyName.ToLower());
                _res.AppendLine();
                _res.Append(TAB3 + "});");
            }

            _res.AppendLine();
            _res.Append(TAB2 + "}");
        }
        private void AddUsing(string entity)
        {
            _res.AppendLine();
            _res.AppendFormat(@"//opath={0}Scripts\Controllers\{1}Ctrl.js,ab=true", Config.Root,entity);
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
            _res.Append("namespace Scripts.Controllers");
            _res.AppendLine();
            _res.Append("{");
            _res.AppendLine();
            _res.AppendFormat(TAB1 + "public interface {0}Scope : IBaseScope<{0}>", entity);
            _res.AppendLine();
            _res.Append(TAB1 + "{");

            foreach (var item in _propList)
            {
                if (item.Details!=null && item.Details.Count > 0)
                {
                    _res.AppendLine();
                    _res.Append(TAB2 + string.Format("List<{0}> {1}", item.Xtype.Substring(item.Xtype.LastIndexOf('.') + 1), item.PropertyName.ToLower()) + "List{get;set;}");

                    this.scopeInitialized.AppendLine();
                    this.scopeInitialized.AppendFormat(TAB3 + "this.scope.{0}List=new List<{1}>();", item.PropertyName.ToLower(), item.Xtype.Substring(item.Xtype.LastIndexOf('.')+1));
                }

                if (item.UiType == "date")
                {
                    hasDatetime = true;
                    dateResolved.AppendLine();
                    dateResolved.AppendFormat(TAB3 + "item.{0}=this.ParseDateTime(item.{0});", item.PropertyName);
                }
            }
            _res.AppendLine();
            _res.Append(TAB1 + "}");
            _res.AppendLine();
            _res.AppendFormat(TAB1 + "[Angular(ModuleName = \"app\", ActionType = \"controller\", ActionName = \"{0}Ctrl\", DI = \"$scope,{0}Service,$sce\")]", entity);
            _res.AppendLine();
            _res.AppendFormat(TAB1 + "public class {0}Ctrl : BaseController<{0}>", entity);
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
