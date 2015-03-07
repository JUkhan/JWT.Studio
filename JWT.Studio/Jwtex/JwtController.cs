using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using jwt.internals;
using System.Reflection;
using jwt.CodeGen;
using System.Text.RegularExpressions;
namespace Jwt.Controller
{
    public class JwtController : BaseController
    {       
        public void Index()
        {
            string nameSpace = GetType().Assembly.GetName().Name;
            string index = "Jwtex.JwtResources.index_jwt.html";
            Response.ContentType = "text/html";
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(index))
            {
                stream.CopyTo(Response.OutputStream);
            }

        }
        public void Resources(string file)
        {
            string ext = file.Substring(file.LastIndexOf(".") + 1);

            using (Stream stream = GetType().Assembly.GetManifestResourceStream("Jwtex.JwtResources." + file))
            {
                switch (ext)
                {
                    case "html":
                        Response.ContentType = "text/html";
                        break;
                    case "css":
                        Response.ContentType = "text/css";
                        break;
                    case "js":
                        Response.ContentType = "text/javascript";
                        break;
                }
                stream.CopyTo(Response.OutputStream);
            }

        }

        #region Layouts
        public JsonResult AddLayout(Jwtex.Layout layout)
        {
            return Json(new { msg = new jwtAppManager(Config.Root).AddLayout(layout) });
        }
        public JsonResult UpdateLayout(Jwtex.Layout layout)
        {
            return Json(new { msg = new jwtAppManager(Config.Root).UpdateLayout(layout) });
        }
        public JsonResult RemoveLayout(Jwtex.Layout layout)
        {
            return Json(new { msg = new jwtAppManager(Config.Root).RemoveLayout(layout) });
        }
        public JsonResult GetLayoutList()
        {
            return Json(new jwtAppManager(Config.Root).GetLayoutList() , JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region Navigtions
        public JsonResult AddNavigation(Jwtex.Navigation navigation)
        {
            return Json(new { msg = new jwtAppManager(Config.Root).AddNavigation(navigation) });
        }
        public JsonResult UpdateNavigation(Jwtex.Navigation navigation)
        {
            return Json(new { msg = new jwtAppManager(Config.Root).UpdateNavigation(navigation) });
        }
        public JsonResult RemoveNavigation(Jwtex.Navigation navigation)
        {
            return Json(new { msg = new jwtAppManager(Config.Root).RemoveNavigation(navigation) });
        }
        public JsonResult GetNavigationList()
        {
            return Json(new jwtAppManager(Config.Root).GetNavigationList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        public JsonResult GenerateConfig()
        {
            try
            {
                new jwtAppManager(Config.Root, GetDefaultNavigation()).GenerateConfig();

            }
             catch (Exception ex)
            {

                return Json(new { msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = "Successfully generated." }, JsonRequestBehavior.AllowGet);
        }
       
        private void createDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
       

        private bool IsExist(string path)
        {
            return System.IO.File.Exists(path);
        }
      
        #region Code Gen

        public JsonResult GetTemplateList()
        {
            JSONData res = new JSONData();
            res.success = true;
            res.data = GetTemplateList(Config.Root);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEntityList()
        {

            JSONData res = new JSONData();
            res.success = false;
            string entityPath = this.Config.EntityProject;
            if (entityPath == "101")
            {
                res.message = "Please provide entity project name into appSettings of web.config file";

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            if (Directory.Exists(entityPath))
            {
                res.data = GetEntityList(entityPath);
                if (res.data == null)
                {
                    res.message = "Could not find the 'Entities' folder in your given entity project name";
                }
                else
                {
                    res.success = true;
                }
            }
            else
            {
                res.success = false;
                res.message = string.Format("'{0}' path is invalid", entityPath);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProperties(string entityName)
        {
            JSONData res = new JSONData();
            res.success = false;
            List<JPropertyInfo> list = new List<JPropertyInfo>();
            List<string> entityList = GetEntityList(Config.EntityProject);
            try
            {

                Assembly assembly = Assembly.Load(Config.EntityModule);
                Type xtype = assembly.GetType(Config.EntityModule + ".Entities." + entityName);
                System.Reflection.PropertyInfo[] propertyList = xtype.GetProperties();

                foreach (System.Reflection.PropertyInfo item in propertyList)
                {
                    JPropertyInfo prop = new JPropertyInfo();
                    string type = item.PropertyType.ToString();
                    if (type.Contains("Collection")) { continue; }
                    prop.PropertyName = item.Name;
                    prop.Xtype = type;
                    prop.HasDetail = !string.IsNullOrEmpty(entityList.FirstOrDefault(x => type.Contains(x)));
                    if (prop.HasDetail)
                    {
                        prop.Details = GetSubProperties(item.Name, assembly, type);
                    }

                    list.Add(prop);
                }
                res.success = true;

                res.data =SyncWithSavedWidget(list, entityName);
            }
            catch (Exception ex)
            {
                res.message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private List<JPropertyInfo> SyncWithSavedWidget(List<JPropertyInfo> list, string widgetName)
        {
            WidgetManager widgetManager = new WidgetManager();
            widgetManager.RootPath = Config.Root;
            JwtWidget temp = widgetManager.GetWidgetByName(widgetName);
            if (temp == null) { return list; }
            foreach (var item in temp.PropertyList)
            {
                JPropertyInfo prop = list.Find(p => p.PropertyName == item.PropertyName);
                if (prop != null)
                {
                    list.Remove(prop);
                    list.Add(item);
                }
            }

            return list;

        }

        public JsonResult CodeGenerate(string entity, List<JPropertyInfo> props)
        {
            try
            {
                string templates = Config.Root + "Templates";
                createDirectory(templates);
                string component = Config.Root + "Templates\\Widgets";
                createDirectory(component);

                //Generate scripts directories
                string script = Config.Root + "\\Scripts";
                createDirectory(script);
                script = Config.Root + "\\Scripts\\Controllers";
                createDirectory(script);
                script = Config.Root + "\\Scripts\\Services";
                createDirectory(script);
                //services
                script = Config.ServiceProject + "\\Interfaces";
                createDirectory(script);
                script = Config.ServiceProject + "\\Implementation";
                createDirectory(script);

                ICode code = new TemplateCode();
                System.IO.File.WriteAllText(component + "\\" + entity + ".html", code.CodeGenerate(entity, props));
                code = new JSController();
                code.Config = this.Config;
                System.IO.File.WriteAllText(Config.Root + "\\Scripts\\Controllers\\" + entity + "Ctrl.js", code.CodeGenerate(entity, props));
                code = new JSService();
                code.Config = this.Config;
                System.IO.File.WriteAllText(Config.Root + "\\Scripts\\Services\\" + entity + "Svc.js", code.CodeGenerate(entity, props));
                code = new CSController();
                System.IO.File.WriteAllText(Config.Root + "Controllers\\" + entity + "Controller.cs", code.CodeGenerate(entity, props));
                code = new CSServiceInterface();
                System.IO.File.WriteAllText(Config.ServiceProject + "\\Interfaces\\I" + entity + "Service.cs", code.CodeGenerate(entity, props));
                code = new CSServiceImplementation();
                System.IO.File.WriteAllText(Config.ServiceProject + "\\Implementation\\" + entity + "Service.cs", code.CodeGenerate(entity, props));

                WidgetManager widgetManager = new WidgetManager();
                widgetManager.RootPath = Config.Root;
                widgetManager.AddWidget(new JwtWidget { Name = entity, PropertyList = props });
                return Json(new { message = "Successfully Generated." });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.ToString() });
            }
        }
        
        private List<JPropertyInfo> GetSubProperties(string entityName, Assembly assembly, string type)
        {
            List<JPropertyInfo> list = new List<JPropertyInfo>();

            Type xtype = assembly.GetType(Config.EntityModule + ".Entities." + entityName);
            if (xtype == null)
            {
                type = type.Substring(type.LastIndexOf(".") + 1);
                xtype = assembly.GetType(Config.EntityModule + ".Entities." + type);
            }
            System.Reflection.PropertyInfo[] propertyList = xtype.GetProperties();

            foreach (System.Reflection.PropertyInfo item in propertyList)
            {
                JPropertyInfo temp = new JPropertyInfo();
                temp.PropertyName = item.Name;
                list.Add(temp);
            }
            return list;
        }
        private List<string> GetEntityList(string path)
        {
            List<string> list = null;
            if (Directory.Exists(path + "//Entities"))
            {
                list = new List<string>();
                foreach (var item in Directory.GetFiles(path + "//Entities"))
                {
                    FileInfo fileInfo = new FileInfo(item);
                    list.Add(fileInfo.Name.Replace(fileInfo.Extension, ""));
                }
            }
            return list;
        }
        private List<string> GetTemplateList(string path)
        {
            List<string> list = null;
            if (Directory.Exists(path + "Templates//Widgets"))
            {
                list = new List<string>();
                foreach (var item in Directory.GetFiles(path + "Templates//Widgets"))
                {
                    FileInfo fileInfo = new FileInfo(item);
                    list.Add(fileInfo.Name.Replace(".html", ""));
                }
            }
            return list;
        }
        #endregion

        public JsonResult GetViewList(string layoutName, string navName)
        {
            JSONData res = new JSONData();
            try
            {
                string input = System.IO.File.ReadAllText(Config.Root + "Templates\\Layouts\\" + layoutName+".html");
                var matches = Regex.Matches(input, "ui-view=\"([a-zA-Z0-9]+)\"", RegexOptions.IgnoreCase);
                List<Jwtex.View> views = new List<Jwtex.View>();
                foreach (Match item in matches)
                {
                    views.Add(new Jwtex.View { ViewName = item.Groups[1].Value, WidgetName="" });
                }
                var nav = new jwtAppManager(Config.Root).GetNavigationList().FirstOrDefault(n => n.NavigationName == navName);
                if (nav != null)
                {
                    if (nav.UIViews != null && nav.UIViews.Count > 0)
                    {
                        foreach (var item in nav.UIViews)
                        {
                            var temp = views.FirstOrDefault(v => v.ViewName == item.ViewName);
                            if (temp != null)
                            {
                                temp.WidgetName = item.WidgetName;
                            }
                        }
                    }
                }
                res.data = views;
                res.success = true;
            }
            catch (Exception ex)
            {

                res.message = ex.ToString();
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
       
    }
    public class JSONData
    {
        public bool success { get; set; }
        public dynamic data { get; set; }
        public string message { get; set; }
    }
    public class JPropertyInfo
    {
        public bool IsReq { get; set; }
        public bool IsMin { get; set; }
        public bool IsMax { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }

        public string MinMsg { get; set; }
        public string MaxMsg { get; set; }
        public string ReqMsg { get; set; }

        public bool Checked { get; set; }
        public string PropertyName { get; set; }
        public bool HasDetail { get; set; }
        public string Xtype { get; set; }

        public string UiType { get; set; }
        public List<JPropertyInfo> Details { get; set; }
    }
    public class JwtWidget
    {
        public string Name { get; set; }
        public List<JPropertyInfo> PropertyList { get; set; }
    }
}
