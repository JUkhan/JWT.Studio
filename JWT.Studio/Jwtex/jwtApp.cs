﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
//using log4net;
using System.Reflection;
using log4net;
using jwt.CodeGen;
using System.Configuration;
namespace Jwtex
{
    public class jwtApp
    {
        public string Name { get; set; }

        public jwtApp()
        {
            this.Name = "app";
        }
        private Dictionary<string, Layout> _layouts = new Dictionary<string, Layout>();
        public Dictionary<string, Layout> GetLayout() { return _layouts; }
        public Dictionary<string, Navigation> GetNavigation() { return _navigations; }
        public Layout Layout
        {
            set
            {
                if (!_layouts.ContainsKey(value.LayoutName))
                {
                    _layouts[value.LayoutName] = value;
                }
            }
        }
        public List<Layout> UILayouts { get; set; }
        public List<Navigation> UINavigations { get; set; }
        private Dictionary<string, Navigation> _navigations = new Dictionary<string, Navigation>();
        public Navigation Navigation
        {
            set
            {
                if (!_navigations.ContainsKey(value.NavigationName))
                {
                    _navigations[value.NavigationName] = value;
                }
            }
        }
        public void Execute()
        {
            new CodeGen(this).Execute();

        }
    }

    public class Layout
    {
        public string LayoutName { get; set; }
        public string Extend { get; set; }
        public bool Abstract { get; set; }
        public string _id { get; set; }
    }
    public class Navigation
    {
        public string _id { get; set; }
        public string NavigationName { get; set; }
        public string HasLayout { get; set; }
        public string ParamName { get; set; }
        public string WidgetName { get; set; }
        private Dictionary<string, View> _views = new Dictionary<string, View>();
        public List<View> UIViews { get; set; }
        public Dictionary<string, View> GetView() { return _views; }
        public View View
        {
            set
            {
                if (!_views.ContainsKey(value.ViewName))
                {
                    _views[value.ViewName] = value;
                }
            }
        }

    }
    public class View
    {
        public string ViewName { get; set; }
        public string WidgetName { get; set; }
    }

    public class CodeGen
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static object locker = new Object();
        private const string TAB1 = "\t";
        private const string TAB2 = "\t\t";
        private const string TAB3 = "\t\t\t";
        private const string TAB4 = "\t\t\t\t";
        public string Root { get; set; }
        public string PathString { get; set; }

        private Dictionary<string, string> navList = new Dictionary<string, string>();
        public CodeGen(jwtApp app)
        {
            this.App = app;
            this.Root = AppDomain.CurrentDomain.BaseDirectory;
        }
        public string GetTemplatePath(string tentativePath, string wigenName)
        {

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["HasTemplateAuthorization"]))
            {
                return tentativePath;
            }
            string path = ConfigurationManager.AppSettings["HasTemplateAuthorization"];
            if (!path.EndsWith("/"))
            {
                path += '/';
            }
            return string.Format("'{0}'", path + wigenName);

        }
        public CodeGen()
        {

        }
        private List<String> mControllers = new List<string>();
        private List<String> layoutControllers = new List<string>();
        public jwtApp App { get; set; }
        public string DefaultNavigation { get; set; }
        public void Execute()
        {
            lock (locker)
            {
                JwtFile file = new JwtFile();
                file.CreateDirectory(Root + "Scripts");
                file.CreateDirectory(Root + "Scripts\\Components");
                file.CreateDirectory(Root + "Scripts\\Layouts");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.Append("export default function config(stateprovider, routeProvider){");
                if (!string.IsNullOrEmpty(DefaultNavigation))
                {
                    sb.AppendLine();
                    sb.AppendFormat(TAB1 + "routeProvider.otherwise('{0}');", DefaultNavigation);
                    sb.AppendLine();
                }
                SetLayout(sb);
                SetNavigation(sb);
                sb.AppendLine();
                sb.Append("}");
                sb.AppendLine();
                sb.Append("config.$inject=['$stateProvider', '$urlRouterProvider'];");
                sb.AppendLine();
                file.Write(this.Root + "Scripts\\config.js", sb.ToString());


                GenAllControllers();
                GenAllServices();
                GenAppDirectives();
                var dir = new DirectoryInfo(Root + "Scripts\\Modules");
                foreach (var item in dir.GetDirectories())
                {
                    if (System.IO.File.Exists(string.Format(Root + "Scripts\\Modules\\{0}\\{0}.css", item.Name)))
                    {
                        componentsCSS.AppendFormat("@import '../Scripts/Modules/{0}/{0}.css';", item.Name);
                        componentsCSS.AppendLine();
                    }
                }
                System.IO.File.WriteAllText(Root + "Content\\components.css", componentsCSS.ToString());
            }
        }
        private StringBuilder componentsCSS = new StringBuilder();
        private void GenAppDirectives()
        {
            DirectoryInfo dir = new DirectoryInfo(Root + "Scripts\\Directives");
            StringBuilder import = new StringBuilder();
            StringBuilder builder = new StringBuilder();

            foreach (var item in dir.GetDirectories())
            {
                import.AppendFormat("import {0} from 'Scripts/Directives/{0}/{0}.js';", item.Name);
                import.AppendLine();

                builder.AppendFormat(".directive('{0}', {0}.builder)", item.Name);
                builder.AppendLine();
                if (System.IO.File.Exists(string.Format(Root + "Scripts/Directives/{0}/{0}.css", item.Name)))
                {
                    componentsCSS.AppendFormat("@import '../Scripts/Directives/{0}/{0}.css';", item.Name);
                    componentsCSS.AppendLine();
                }
            }

            StringBuilder res = new StringBuilder();
            res.Append(import);
            res.AppendLine();
            res.AppendLine();
            res.Append("var moduleName='app.Directives';");
            res.AppendLine();
            res.AppendLine();
            res.Append("angular.module(moduleName, [])");
            res.AppendLine();
            res.Append(builder);
            res.Append(";");
            res.AppendLine();
            res.AppendLine();
            res.Append("export default moduleName;");
            System.IO.File.WriteAllText(Root + "Scripts\\app.directives.js", res.ToString());
            // System.IO.File.WriteAllText(Root + "Content\\components.css", componentsCSS.ToString());
        }
        private void GetNamArr(StringBuilder sb)
        {
            bool isFirst = true;
            foreach (var item in navList.Values)
            {
                if (!isFirst) { sb.Append(','); }
                sb.Append(item);
                isFirst = false;
            }
        }
        private void SetLayout(StringBuilder sb)
        {
            JwtFile file = new JwtFile();
            foreach (Layout item in App.GetLayout().Values)
            {
                file.CreateDirectory(Root + "Scripts\\Layouts\\" + item.LayoutName);
                sb.AppendLine();
                sb.Append(TAB1);
                sb.AppendFormat("stateprovider.state('{0}'", GetStateName(item));
                sb.Append(",{");
                if (item.Abstract)
                    sb.Append("abstract:true,");
                sb.AppendFormat(@"url:'/{0}'", item.LayoutName);

                PathString = Root + string.Format("Scripts\\Layouts\\{0}\\{0}.html", item.LayoutName);
                if (!file.FileExists(PathString))
                {
                    file.Write(PathString, string.Format("<h3>Layout Name : {0}</h3><div ui-view></div>", item.LayoutName));
                }
                sb.Append(",templateUrl:" + GetTemplatePath(string.Format("'Scripts/Layouts/{0}/{0}.html'", item.LayoutName), item.LayoutName + "__LAYOUT__"));


                PathString = Root + string.Format("Scripts\\Layouts\\{0}\\{0}Ctrl.js", item.LayoutName);
                if (!file.FileExists(PathString))
                {
                    file.Write(PathString, getEmptyControllerForLayout(item.LayoutName));
                }

                sb.AppendFormat(",controller:'{0}Ctrl as vm'", item.LayoutName);
                layoutControllers.Add(item.LayoutName);
                sb.Append(HasResolver(file, Root + "Scripts\\Layouts\\" + item.LayoutName));
                sb.Append("});");
            }

        }
        private string HasResolver(JwtFile file, string basePath)
        {
            if (file.FileExists(basePath + "\\resolve.js"))
            {
                string fileContent = file.Read(basePath + "\\resolve.js");
                fileContent = fileContent.Trim();
                fileContent = fileContent.Substring(fileContent.IndexOf("{"));
                if (fileContent.EndsWith(";"))
                    fileContent = fileContent.Substring(0, fileContent.Length - 1);
                return ",resolve:" + fileContent;
            }
            return "";

        }
        private void SetNavigation(StringBuilder sb)
        {
            JwtFile file = new JwtFile();
            sb.AppendLine();
            foreach (var item in App.GetNavigation().Values)
            {
                bool createNew = false;
                if (!file.DirectoryExists(Root + "Scripts\\Components\\" + item.WidgetName))
                {
                    createNew = true;
                }
                file.CreateDirectory(Root + "Scripts\\Components\\" + item.WidgetName);
                sb.AppendLine();
                sb.Append(TAB1);
                sb.AppendFormat("stateprovider.state('{0}'", GetStateName(item));
                sb.Append(",{");
                sb.AppendFormat(@"url:'/{0}{1}'", item.NavigationName, string.IsNullOrEmpty(item.ParamName) ? "" : GetParamName(item.ParamName));

                var view = item.GetView();
                if (view != null && item.GetView().Count > 0)
                {
                    sb.Append(",views:{");
                    bool isFirst = true;
                    foreach (var item2 in view.Values)
                    {
                        if (isFirst)
                            sb.Append("'" + item2.ViewName + "':{");
                        else
                            sb.Append(",'" + item2.ViewName + "':{");
                        if (!string.IsNullOrEmpty(item2.WidgetName))
                        {
                            PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}.html", item2.WidgetName);
                            if (!file.FileExists(PathString))
                            {
                                file.Write(PathString, "<h3>widget Name : {{vm.title}}</h3>");
                            }
                            sb.Append("templateUrl:" + GetTemplatePath(string.Format("'Scripts/Components/{0}/{0}.html'", item2.WidgetName), item2.WidgetName));
                            mControllers.Add(item2.WidgetName);

                            PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}Ctrl.js", item2.WidgetName);
                            if (file.FileExists(PathString))
                            {
                                sb.AppendFormat(",controller:'{0}Ctrl as vm'", item2.WidgetName);
                            }

                        }
                        sb.Append(HasResolver(file, Root + "Scripts\\Components\\" + item2.WidgetName));
                        sb.Append("}");
                        isFirst = false;
                    }
                    sb.Append("}");
                }
                if (!string.IsNullOrEmpty(item.WidgetName))
                {
                    if (createNew)
                    {
                        PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}.html", item.WidgetName);
                        file.Write(PathString, "<h3>widget Name : {{vm.title}}</h3>");
                        PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}Ctrl.js", item.WidgetName);
                        file.Write(PathString, getEmptyController(item.WidgetName));
                        PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}Svc.js", item.WidgetName);
                        file.Write(PathString, getEmptyService(item.WidgetName));
                    }
                    PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}.html", item.WidgetName);
                    if (file.FileExists(PathString))
                    {
                        sb.Append(",templateUrl:" + GetTemplatePath(string.Format("'Scripts/Components/{0}/{0}.html'", item.WidgetName), item.WidgetName));
                    }
                    PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}Ctrl.js", item.WidgetName);
                    if (file.FileExists(PathString))
                    {
                        sb.AppendFormat(",controller:'{0}Ctrl as vm'", item.WidgetName);
                    }
                    mControllers.Add(item.WidgetName);
                }
                sb.Append(HasResolver(file, Root + "Scripts\\Components\\" + item.WidgetName));
                sb.Append("});");
            }

        }

        private string GetParamName(string p)
        {
            if (p.StartsWith("/:")) return p;
            if (p.StartsWith(":")) return '/' + p;
            return "/:" + p;
        }

        private void GenAllControllers()
        {
            var list = mControllers.Distinct();
            StringBuilder sb = new StringBuilder();
            var directoryName = "Components";
            foreach (var item in list)
            {
                if (File.Exists(Root + string.Format("Scripts\\Components\\{0}\\{0}Ctrl.js", item)))
                {
                    sb.AppendLine();
                    sb.AppendFormat("import {0} from 'Scripts/{1}/{0}/{0}Ctrl.js';", item, directoryName);
                }
            }
            list = layoutControllers.Distinct();
            directoryName = "Layouts";
            foreach (var item in list)
            {
                if (File.Exists(Root + string.Format("Scripts\\Layouts\\{0}\\{0}Ctrl.js", item)))
                {
                    sb.AppendLine();
                    sb.AppendFormat("import {0} from 'Scripts/{1}/{0}/{0}Ctrl.js';", item, directoryName);
                }
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendFormat("var moduleName='{0}.controllers';", this.App.Name);
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("angular.module(moduleName,[])");
            list = mControllers.Distinct();
            foreach (var item in list)
            {
                if (File.Exists(Root + string.Format("Scripts\\Components\\{0}\\{0}Ctrl.js", item)))
                {
                    sb.AppendLine();
                    sb.AppendFormat(".controller('{0}Ctrl', {0})", item);
                }
                if (System.IO.File.Exists(string.Format(Root + "Scripts\\Components/{0}/{0}.css", item)))
                {
                    componentsCSS.AppendFormat("@import '../Scripts/Components/{0}/{0}.css';", item);
                    componentsCSS.AppendLine();
                }
            }
            list = layoutControllers.Distinct();
            foreach (var item in list)
            {
                if (File.Exists(Root + string.Format("Scripts\\Layouts\\{0}\\{0}Ctrl.js", item)))
                {
                    sb.AppendLine();
                    sb.AppendFormat(".controller('{0}Ctrl', {0})", item);
                }
                if (System.IO.File.Exists(string.Format(Root + "Scripts\\Layouts/{0}/{0}.css", item)))
                {
                    componentsCSS.AppendFormat("@import '../Scripts/Layouts/{0}/{0}.css';", item);
                    componentsCSS.AppendLine();
                }
            }
            sb.Append(";");
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("export default moduleName;");
            File.WriteAllText(Root + "Scripts\\app.controllers.js", sb.ToString());
        }

        private void GenAllServices()
        {
            var list = mControllers.Distinct();
            StringBuilder sb = new StringBuilder();
            var directoryName = "Components";
            foreach (var item in list)
            {
                if (File.Exists(Root + string.Format("Scripts\\Components\\{0}\\{0}Svc.js", item)))
                {
                    sb.AppendLine();
                    sb.AppendFormat("import {0} from 'Scripts/{1}/{0}/{0}Svc.js';", item, directoryName);
                }
            }
            list = layoutControllers.Distinct();
            directoryName = "Layouts";
            foreach (var item in list)
            {
                if (File.Exists(Root + string.Format("Scripts\\Layouts\\{0}\\{0}Svc.js", item)))
                {
                    sb.AppendLine();
                    sb.AppendFormat("import {0} from 'Scripts/{1}/{0}/{0}Svc.js';", item, directoryName);
                }
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendFormat("var moduleName='{0}.services';", this.App.Name);
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("angular.module(moduleName,[])");
            list = mControllers.Distinct();
            foreach (var item in list)
            {
                if (File.Exists(Root + string.Format("Scripts\\Components\\{0}\\{0}Svc.js", item)))
                {
                    sb.AppendLine();
                    sb.AppendFormat(".factory('{0}Svc', {0})", item);
                }
            }
            list = layoutControllers.Distinct();
            foreach (var item in list)
            {
                if (File.Exists(Root + string.Format("Scripts\\Layouts\\{0}\\{0}Svc.js", item)))
                {
                    sb.AppendLine();
                    sb.AppendFormat(".factory('{0}Svc', {0})", item);
                }
            }
            sb.Append(";");
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("export default moduleName;");
            File.WriteAllText(Root + "Scripts\\app.services.js", sb.ToString());
        }

        public string getEmptyController(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("import BaseCtrl from 'Scripts/Base/BaseCtrl.js';");
            sb.AppendLine();
            //sb.Append("const SVC=new WeakMap();");
            sb.AppendLine();
            sb.AppendFormat("class {0}Ctrl extends BaseCtrl", name);
            sb.AppendLine();
            sb.Append("{");
            sb.Append(Environment.NewLine + TAB1 + "constructor(scope, svc){");
            sb.Append(Environment.NewLine + TAB2 + "super(scope);");
            //sb.Append(Environment.NewLine + TAB2 + "SVC.set(this, svc);");
            sb.Append(Environment.NewLine + TAB2 + "this.svc = svc;");
            sb.AppendFormat(Environment.NewLine + TAB2 + "this.title='{0}';", name);
            sb.Append(Environment.NewLine + TAB1 + "}");
            sb.AppendLine();
            sb.Append("}");
            sb.AppendFormat(Environment.NewLine + "{0}Ctrl.$inject=['$scope', '{0}Svc'];", name);
            sb.AppendFormat(Environment.NewLine + "export default {0}Ctrl;", name);
            return sb.ToString();
        }
        public string getEmptyControllerForLayout(string name)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("class {0}Ctrl", name);
            sb.AppendLine();
            sb.Append("{");
            sb.Append(Environment.NewLine + TAB1 + "constructor(){");
            sb.AppendFormat(Environment.NewLine + TAB2 + "this.title='{0}';", name);
            sb.Append(Environment.NewLine + TAB1 + "}");
            sb.AppendLine();
            sb.Append("}");
            sb.AppendFormat(Environment.NewLine + "export default {0}Ctrl;", name);
            return sb.ToString();
        }
        public string getEmptyDirective(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("class {0}", name);
            sb.AppendLine();
            sb.Append("{");
            sb.Append(Environment.NewLine + TAB1 + "constructor(){");
            sb.Append(Environment.NewLine + TAB2 + "this.restrict='E';");
            sb.AppendFormat(Environment.NewLine + TAB2 + "this.templateUrl='Scripts/Directives/{0}/{0}.html';", name);
            sb.Append(Environment.NewLine + TAB1 + "}");
            sb.AppendLine();

            sb.Append(TAB1 + "static builder()");
            sb.Append(TAB1 + "{");
            sb.AppendLine();
            sb.AppendFormat(TAB2 + "return new {0}();", name);
            sb.AppendLine();
            sb.Append(TAB1 + "}");
            sb.AppendLine();
            sb.Append("}");
            sb.AppendFormat(Environment.NewLine + "export default {0};", name);
            return sb.ToString();
        }
        public string getEmptyService(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("import BaseSvc from 'Scripts/Base/BaseSvc.js';");
            sb.AppendLine();
            //sb.Append("const HTTP=new WeakMap();");
            sb.AppendLine();
            sb.AppendFormat("class {0}Svc extends BaseSvc", name);
            sb.AppendLine();
            sb.Append("{");
            sb.Append(Environment.NewLine + TAB1 + "constructor(http){");
            sb.Append(Environment.NewLine + TAB2 + "super(http);");
            //sb.Append(Environment.NewLine + TAB2 + "HTTP.set(this, http);");
            sb.Append(Environment.NewLine + TAB2 + "this.http= http;");
            sb.Append(Environment.NewLine + TAB1 + "}");
            sb.AppendLine();
            var cname = name[0].ToString().ToLower() + name.Substring(1);
            sb.AppendFormat(TAB1 + "static {0}Factory(http)", cname);
            sb.Append(TAB1 + "{");
            sb.AppendLine();
            sb.AppendFormat(TAB2 + "return new {0}Svc(http);", name);
            sb.AppendLine();
            sb.Append(TAB1 + "}");
            sb.AppendLine();
            sb.Append("}");
            sb.AppendFormat(Environment.NewLine + "{0}Svc.{1}Factory.$inject=['$http'];", name, cname);
            sb.AppendFormat(Environment.NewLine + "export default {0}Svc.{1}Factory;", name, cname);
            return sb.ToString();
        }
        private string GetStateName(Navigation navigation)
        {
            List<string> nameList = new List<string>();
            nameList.Add(navigation.NavigationName);
            Layout layout = null;
            if (!string.IsNullOrEmpty(navigation.HasLayout))
            {
                layout = App.GetLayout()[navigation.HasLayout];
                nameList.Add(layout.LayoutName);
                while (!string.IsNullOrEmpty(layout.Extend))
                {
                    layout = App.GetLayout()[layout.Extend];
                    nameList.Add(layout.LayoutName);
                }
            }
            navList[navigation.NavigationName] = string.Format("'{0}':['{1}','{2}']", navigation.NavigationName, Reverse(nameList, "/"), navigation.ParamName ?? "");
            return Reverse(nameList);
        }

        private string GetStateName(Layout layout)
        {
            List<string> nameList = new List<string>();
            nameList.Add(layout.LayoutName);
            while (!string.IsNullOrEmpty(layout.Extend))
            {
                layout = App.GetLayout()[layout.Extend];
                nameList.Add(layout.LayoutName);
            }
            return Reverse(nameList);
        }
        private string Reverse(List<string> inputList, string joinBy = ".")
        {
            string res = "", joinWith = "";

            for (int i = inputList.Count - 1; i >= 0; i--)
            {
                res += joinWith + inputList[i];
                if (string.IsNullOrEmpty(joinWith)) { joinWith = joinBy; }
            }
            return res;
        }


        private void InstallTplFile(JwtFile file, string fileName, string path)
        {
            try
            {
                lock (locker)
                {
                    using (Stream stream = GetType().Assembly.GetManifestResourceStream("Jwtex.JwtResources.tpl." + fileName))
                    {
                        string data = file.StreamToString(stream);
                        file.Write(path, data);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        public void Startup()
        {
            JwtFile file = new JwtFile();
            lock (locker)
            {
                file.CreateDirectory(Root + "Scripts");
                file.CreateDirectory(Root + "Scripts\\Modules");
                file.CreateDirectory(Root + "Scripts\\Components");
                file.CreateDirectory(Root + "Scripts\\Directives");
                if (!file.DirectoryExists(Root + "Scripts\\Base"))
                {
                    file.CreateDirectory(Root + "Scripts\\Base");
                    file.CreateDirectory(Root + "Scripts\\Filters");
                    //root
                    InstallTplFile(file, "jwtApp.txt", Root + "jwtApp.config");
                    InstallTplFile(file, "authComplete.html", Root + "authComplete.html");
                    //Scripts

                    InstallTplFile(file, "appStarter.js", Root + "Scripts\\appStarter.js");
                    InstallTplFile(file, "app.controllers.js", Root + "Scripts\\app.controllers.js");
                    InstallTplFile(file, "app.directives.js", Root + "Scripts\\app.directives.js");
                    InstallTplFile(file, "app.services.js", Root + "Scripts\\app.services.js");
                    InstallTplFile(file, "filters.js", Root + "Scripts\\app.filters.js");
                    InstallTplFile(file, "config.js", Root + "Scripts\\config.js");
                    //Scripts/Base 
                    InstallTplFile(file, "Base.app.js", Root + "Scripts\\Base\\app.js");
                    InstallTplFile(file, "Base.BaseCtrl.js", Root + "Scripts\\Base\\BaseCtrl.js");
                    InstallTplFile(file, "Base.BaseSvc.js", Root + "Scripts\\Base\\BaseSvc.js");
                    InstallTplFile(file, "Base.authService.js", Root + "Scripts\\Base\\authService.js");
                    InstallTplFile(file, "Base.authInterceptorService.js", Root + "Scripts\\Base\\authInterceptorService.js");

                    //Scripts/Components
                    //home
                    file.CreateDirectory(Root + "Scripts\\Components\\home");
                    InstallTplFile(file, "home.home.html", Root + "Scripts\\Components\\home\\home.html");
                    InstallTplFile(file, "home.homeCtrl.js", Root + "Scripts\\Components\\home\\homeCtrl.js");
                    InstallTplFile(file, "home.homeSvc.js", Root + "Scripts\\Components\\home\\homeSvc.js");
                    //login
                    file.CreateDirectory(Root + "Scripts\\Components\\login");
                    InstallTplFile(file, "login.login.html", Root + "Scripts\\Components\\login\\login.html");
                    InstallTplFile(file, "login.loginCtrl.js", Root + "Scripts\\Components\\login\\loginCtrl.js");

                    //signup
                    file.CreateDirectory(Root + "Scripts\\Components\\signup");
                    InstallTplFile(file, "signup.signup.html", Root + "Scripts\\Components\\signup\\signup.html");
                    InstallTplFile(file, "signup.signupCtrl.js", Root + "Scripts\\Components\\signup\\signupCtrl.js");

                    //associate
                    file.CreateDirectory(Root + "Scripts\\Components\\associate");
                    InstallTplFile(file, "associate.associate.html", Root + "Scripts\\Components\\associate\\associate.html");
                    InstallTplFile(file, "associate.associateCtrl.js", Root + "Scripts\\Components\\associate\\associateCtrl.js");


                    InstallTplFile(file, "jwtDate.txt", Root + "Scripts\\Filters\\jwtDate.js");
                    if (file.FileExists(Root + "Views\\Shared\\_Layout.cshtml"))
                        InstallTplFile(file, "layout.txt", Root + "Views\\Shared\\_Layout.cshtml");

                    file.CreateDirectory(Root + "Scripts\\Layouts");
                    file.CreateDirectory(Root + "Scripts\\Layouts\\root");
                    InstallTplFile(file, "root.txt", Root + "Scripts\\Layouts\\root\\root.html");
                    InstallTplFile(file, "rootCtrl.txt", Root + "Scripts\\Layouts\\root\\rootCtrl.js");

                    file.CreateDirectory(Root + "Scripts\\Directives\\jwtFilter");
                    InstallTplFile(file, "jwtFilter.txt", Root + "Scripts\\Directives\\jwtFilter\\jwtFilter.js");
                    if (file.DirectoryExists(Root + "Views\\Home"))
                    {
                        file.Write(Root + "Views\\Home\\Index.cshtml", "<b>Loading...</b>");
                    }
                }
            }
        }
        public void CreateItem(string mode, string name)
        {
            JwtFile file = new JwtFile();
            lock (locker)
            {
                file.CreateDirectory(Root + "Scripts");
                file.CreateDirectory(Root + "Scripts\\Components");
                file.CreateDirectory(Root + "Scripts\\Directives");
                file.CreateDirectory(Root + "Scripts\\Modules");
                try
                {
                    string path = Root;
                    switch (mode)
                    {
                        case "Widgets":
                            path += "Scripts\\Components\\" + name;
                            file.CreateDirectory(path);
                            path = Root + string.Format("Scripts\\Components\\{0}\\{0}Ctrl.js", name);
                            file.Write(path, getEmptyController(name));
                            path = Root + string.Format("Scripts\\Components\\{0}\\{0}Svc.js", name);
                            file.Write(path, getEmptyService(name));
                            path = Root + string.Format("Scripts\\Components\\{0}\\{0}.html", name);
                            file.Write(path, "<h3>widget Name : {{vm.title}}</h3>");

                            break;
                        case "Components":
                            path += "Scripts\\Directives\\" + name;
                            file.CreateDirectory(path);
                            path = Root + string.Format("Scripts\\Directives\\{0}\\{0}.js", name);
                            file.Write(path, getEmptyDirective(name));
                            path = Root + string.Format("Scripts\\Directives\\{0}\\{0}.html", name);
                            file.Write(path, "<b>Hello world</b>");
                            path = Root + string.Format("Scripts\\Directives\\{0}\\{0}.css", name);
                            file.Write(path, "/*css goes here*/");
                            break;
                        case "Modules":
                            if (name == "app") return;
                            path += "Scripts\\Modules\\" + name;
                            file.CreateDirectory(path);
                            path = Root + string.Format("Scripts\\Modules\\{0}\\{0}.js", name);
                            file.Write(path, getEmptyModule(name));
                            break;
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex);

                }
            }

        }

        private string getEmptyModule(string name)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("//import sample from 'Scripts/Modules/{0}/sample.js';", name);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendFormat("var moduleName='{0}'; ", name);
            sb.AppendLine();
            sb.Append("angular.module(moduleName, []);");
            sb.AppendLine();
            sb.Append("export default moduleName;");
            sb.AppendLine();
            return sb.ToString();
        }
    }

}
