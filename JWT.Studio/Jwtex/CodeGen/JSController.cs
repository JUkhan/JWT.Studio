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
            _res.Append("}");
            _res.AppendLine();
            _res.AppendFormat("{0}Ctrl.$inject=['{0}Svc', '$scope'];", entity);
            _res.AppendLine();
            _res.AppendFormat("export default {0}Ctrl;",entity);
            return _res.ToString();
        }
        private bool hasDatetime = false;
        private StringBuilder scopeInitialized = new StringBuilder();
        private StringBuilder dateResolved = new StringBuilder();
        private void AddConstructor(string entity)
        {
            var xentity = entity.Length > 1 ? entity[0].ToString().ToLower() + entity.Substring(1) : entity;
            _res.AppendLine();
            _res.AppendFormat(TAB1 + "constructor({0}Svc, scope)", xentity);
            _res.AppendLine();
            _res.Append(TAB1 + "{");

            _res.AppendLine();
            _res.AppendFormat(TAB2 + "super({0}Svc, scope);", xentity);
            _res.AppendLine();

            _res.AppendLine();
            _res.AppendFormat(TAB2 + "SVC.set(this, {0}Svc);",xentity);
            _res.AppendLine();

            _res.Append(this.scopeInitialized.ToString());

            _res.AppendLine();
            _res.Append(TAB2 + "scope.gridOpts.columnDefs =[");

            _res.AppendLine();
            _res.Append(TAB3 + "{ name:\"AC\",  width:50, enableSorting:false, cellTemplate:\"<div style='text-align:center'><a ng-click=\\\"getExternalScopes().editAction(row)\\\" href=\\\"javascript:;\\\"> <i class=\\\"fa fa-pencil\\\"></i>  </a><a ng-click=\\\"getExternalScopes().removeAction(row)\\\" href=\\\"javascript:;\\\"> <i class=\\\"fa fa-trash\\\"></i>  </a></div>\"}");
            foreach (var item in _propList)
            {
                if (item.Checked)
                {

                    if (item.UiType == "date")
                    {
                        _res.AppendLine();
                        _res.Append(TAB4 + ",{ name:\"" + item.PropertyName + "\" ,cellFilter:\"jwtDate | date:'yyyy-MM-dd'\"}");
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
                                _res.Append(TAB4 + ",{ name:\""+temp.PropertyName+"\", field:\""+temp.PropertyName+"_"+x.PropertyName +"\"}");
                            }
                        }
                    }
                    else
                    {                        
                        _res.AppendLine();
                        _res.Append(TAB4 + ",{ name:\""+item.PropertyName+"\"}");
                    }
                }
            }
            _res.AppendLine();
            _res.Append(TAB3 + "];");
            _res.AppendLine();
            _res.Append(TAB3+ "scope.gridOpts.onRegisterApi = gridApi => { ");
            _res.AppendLine();
            _res.Append(TAB4 + "gridApi.paging.on.pagingChanged(scope,(newPage, pageSize)=>{");
                _res.AppendLine();
                _res.Append(TAB5 + "this.pageNo = newPage;");
                _res.AppendLine();
                _res.Append(TAB5 + "this.pageSize = pageSize;");
                _res.AppendLine();
                _res.Append(TAB5 + "this.getPagedList();");
                _res.AppendLine();
                _res.Append(TAB4 + "});");
                _res.AppendLine();
                _res.Append(TAB3 + "};");

            _res.AppendLine();
            _res.Append(TAB2 + "this.getPagedList();");
            _res.AppendLine();
            _res.Append(TAB2 + "this.loadRelationalData();");
            _res.AppendLine();
            _res.Append(TAB1 + "}");

        }
        public void AddRemoveMethod(string entity)
        {
            _res.AppendLine();
            _res.Append(TAB1 + "onAfterDeleted(item)");
            _res.AppendLine();
            _res.Append(TAB1 + "{");

            _res.AppendLine();
            _res.AppendFormat(TAB2 + "this.getScope().list.remove(x => x.{0}ID == item.{0}ID);", entity);
            _res.AppendLine();
            _res.Append(TAB1 + "}");

            

            //loadRelationalData
            var list = _propList.Where(n => n.Details != null && n.Details.Count > 1);
            _res.AppendLine();
            _res.Append(TAB1 + "loadRelationalData()");
            _res.AppendLine();
            _res.Append(TAB1 + "{");

            foreach (var item in list)
            {
                _res.AppendLine();
                _res.AppendFormat(TAB2 + "SVC.get(this).get{0}List().success(res =>", item.PropertyName);
                _res.AppendLine();
                _res.Append(TAB2+"{");
                _res.AppendLine();
                _res.AppendFormat(TAB3 + "this.{0}List=res.Data;", item.PropertyName.ToLower());
                _res.AppendLine();
                _res.Append(TAB2 + "});");
            }

            _res.AppendLine();
            _res.Append(TAB1 + "}");

           
        }
        private void AddUsing(string entity)
        {         
            _res.AppendLine();
            _res.Append("import BaseEntityCtrl from 'Scripts/base/BaseEntityCtrl.js';");
            _res.AppendLine();
            _res.AppendFormat("const SVC=new WeakMap();");
            _res.AppendLine();

            _res.AppendFormat("class {0}Ctrl extends BaseEntityCtrl", entity);
            _res.AppendLine();
            _res.AppendLine("{");

        }

        public JwtConfig Config
        {
            get;
            set;
        }
    }

}
