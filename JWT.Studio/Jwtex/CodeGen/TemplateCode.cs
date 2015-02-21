using Jwt.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwt.CodeGen
{
    public class TemplateCode : CodeGen, ICode
    {
        public string CodeGenerate(string entity, List<JPropertyInfo> props)
        {
            _propList = props;
            SetGrid();
            _res.AppendLine();
            _res.Append("<div class=\"container\" ng-hide=\"!vm.isGrid\">");
            _res.AppendLine();
            _res.Append(TAB1 + "<div class=\"col-sm-8 col-sm-offset-2\">");
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "<div class=\"page-header\"><b>{0}</b></div>", entity);
            _res.AppendLine();
            _res.AppendFormat(TAB2 + "<form name=\"{0}Form\" ng-submit=\"vm.submitForm()\" novalidate>", entity);
            foreach (JPropertyInfo item in props)
            {
                if (item.Checked)
                {
                    if (item.UiType == "select")
                    {
                        _res.AppendLine();
                        _res.AppendFormat(TAB3 + " <div class=\"form-group\" ng-class=\"{0} 'has-error' : {1}Form.{2}.$invalid && !{1}Form.{2}.$pristine {3}\">", "{", entity, item.PropertyName,  "}");

                        _res.AppendLine();
                        _res.AppendFormat(TAB4 + "<label>{0}</label>", item.PropertyName.Substring(0, item.PropertyName.Length-2));

                        _res.AppendLine();

                        _res.AppendFormat(TAB4 + "<select  name=\"{0}\" class=\"form-control\"  ng-model=\"vm.model.{1}\" {2} {3} > <option value=\"\">--select--</option></select>", item.PropertyName, item.PropertyName, getValidation(item), getSelectQuery(item));


                        _res.AppendLine();
                        _res.Append(TAB3 + "</div>");

                    }
                    else
                    {
                        _res.AppendLine();
                        _res.AppendFormat(TAB3 + " <div class=\"form-group\" ng-class=\"{0} 'has-error' : {1}Form.{2}.$invalid && !{1}Form.{2}.$pristine {3}\">", "{", entity, item.PropertyName, "}");

                        _res.AppendLine();
                        _res.AppendFormat(TAB4 + "<label>{0}</label>", item.PropertyName);

                        _res.AppendLine();
                        if (item.UiType == "textarea")
                        {
                            _res.AppendFormat(TAB4 + "<textarea  name=\"{0}\" class=\"form-control\"  ng-model=\"vm.model.{1}\" {2}  ></textarea>", item.PropertyName, item.PropertyName, getValidation(item));
                        }
                        else
                        {
                            _res.AppendFormat(TAB4 + "<input type=\"{0}\" name=\"{1}\" class=\"form-control\"  ng-model=\"vm.model.{2}\" {3}  />", item.UiType, item.PropertyName, item.PropertyName, getValidation(item));
                        }
                        //validation message
                        if (item.IsReq)
                        {
                            _res.AppendLine();
                            _res.AppendFormat(TAB4 + "<p ng-show=\"{0}Form.{1}.$invalid && !{2}Form.{3}.$pristine\" class=\"help-block\">{4}</p>", entity, item.PropertyName, entity, item.PropertyName, item.ReqMsg);
                        }
                        if (item.IsMin)
                        {
                            _res.AppendLine();
                            _res.AppendFormat(TAB4 + "<p ng-show=\"{0}Form.{1}.$error.minlength\" class=\"help-block\">{2}</p>", entity, item.PropertyName, item.MinMsg);
                        }
                        if (item.IsMax)
                        {
                            _res.AppendLine();
                            _res.AppendFormat(TAB4 + "<p ng-show=\"{0}Form.{1}.$error.maxlength\" class=\"help-block\">{2}</p>", entity, item.PropertyName, item.MaxMsg);
                        }
                        _res.AppendLine();
                        _res.Append(TAB3 + "</div>");
                    }
                }
            }

            _res.AppendLine();
            _res.AppendFormat(TAB3 + "<button type=\"submit\" class=\"btn btn-primary\" ng-disabled=\"{0}Form.$invalid\">Submit</button>", entity);
            _res.AppendLine();
            _res.Append(TAB3 + "<button type=\"button\" class=\"btn btn-default\" ng-click=\"vm.isGrid=false\">Cancel</button>");
            _res.AppendLine();
            _res.Append(TAB2 + "</form>");
            _res.AppendLine();
            _res.Append(TAB1 + "</div>");
            _res.AppendLine();
            _res.Append("</div>");

            return _res.ToString();
        }

        private string getSelectQuery(JPropertyInfo item)
        {
            string res = ""; bool isAbnrmal = false;
            string field = item.PropertyName.ToLower().Replace("id", "");
            JPropertyInfo temp = _propList.FirstOrDefault(x => x.PropertyName.ToLower() == field);
            if (temp == null)
            {
                temp = _propList.FirstOrDefault(x => x.Xtype.ToLower().Contains(field));
                isAbnrmal = true;
            }
            if (temp != null)
            {
                if (temp.Details != null && temp.Details.Count > 0)
                {
                    int count = 0; string id = "", val="";
                    foreach (JPropertyInfo pro in temp.Details)
                    {
                        if (pro.Checked)
                        {
                            if ((pro.PropertyName.ToLower() == field + "id") || (pro.PropertyName.ToLower() == "id"))
                            {
                                count++; id = pro.PropertyName;
                            }
                            else
                            {
                                count++; val = pro.PropertyName;
                            }
                        }
                        if (count >= 2) { break; }
                    }
                    if (isAbnrmal)
                    {
                        res = string.Format(" ng-options=\"s.{0} as s.{1} for s in vm.{2}List\"", id, val, temp.PropertyName.ToLower());
                    }
                    else
                    {
                        res = string.Format(" ng-options=\"s.{0} as s.{1} for s in vm.{2}List\"", id, val, field);
                    }
                }
            }
            return res;
        }

        private string getValidation(JPropertyInfo item)
        {
            string res = "";
            if (item.IsReq) { res += "required"; }
            if (item.IsMin) { res += string.Format(" ng-minlength=\"{0}\"", item.MinLength); }
            if (item.IsMax) { res += string.Format(" ng-maxlength=\"{0}\"", item.MaxLength); }
            return res;
        }
        private void SetGrid()
        {
            _res.Append("<div class=\"container\" ng-hide=\"vm.isGrid\">");
            _res.AppendLine();
            _res.Append(TAB1 + "<input type=\"button\" class=\"btn btn-link\" value=\"Create New\" ng-click=\"vm.createNewItem()\" />");
            _res.AppendLine();
            _res.AppendFormat(TAB1 + "<div id=\"grid-{0}\" ui-grid=\"gridOpts\" ui-grid-paging external-scopes=\"gridAction\" class=\"grid\"></div>", Guid.NewGuid().ToString());
            _res.AppendLine();
            _res.Append("</div>");
        }

        public JwtConfig Config
        {
            get;
            set;
        }
    }
}
