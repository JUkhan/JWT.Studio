﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
//using log4net;
using System.Reflection;
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
        //private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
        public CodeGen()
        {

        }
        private List<String> mControllers = new List<string>();
        private List<String> layoutControllers = new List<string>();
        public jwtApp App { get; set; }
        public string DefaultNavigation { get; set; }
        public void Execute()
        {
            CreateDirectory(Root + "Scripts");
            CreateDirectory(Root + "Scripts\\Components");
            CreateDirectory(Root + "Scripts\\Layouts");

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
            System.IO.File.WriteAllText(this.Root + "Scripts\\config.js", sb.ToString());


            GenAllControllers();
            GenAllServices();
            GenAppDirectives();
        }
        private void GenAppDirectives()
        {
            DirectoryInfo dir = new DirectoryInfo(Root + "Scripts\\Directives");
            StringBuilder import = new StringBuilder();
            StringBuilder builder = new StringBuilder();
            StringBuilder componentsCSS = new StringBuilder();
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
            System.IO.File.WriteAllText(Root + "Content\\components.css", componentsCSS.ToString());
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

            foreach (Layout item in App.GetLayout().Values)
            {
                CreateDirectory(Root + "Scripts\\Layouts\\" + item.LayoutName);
                sb.AppendLine();
                sb.Append(TAB1);
                sb.AppendFormat("stateprovider.state('{0}'", GetStateName(item));
                sb.Append(",{abstract:true,");
                sb.AppendFormat(@"url:'/{0}'", item.LayoutName);

                PathString = Root + string.Format("Scripts\\Layouts\\{0}\\{0}.html", item.LayoutName);
                if (!File.Exists(PathString))
                {
                    File.WriteAllText(PathString, string.Format("<h3>Layout Name : {0}</h3><div ui-view></div>", item.LayoutName));
                }
                sb.AppendFormat(",templateUrl:'Scripts/Layouts/{0}/{0}.html'", item.LayoutName);


                PathString = Root + string.Format("Scripts\\Layouts\\{0}\\{0}Ctrl.js", item.LayoutName);
                if (!File.Exists(PathString))
                {
                    File.WriteAllText(PathString, getEmptyController(item.LayoutName));
                }
                sb.AppendFormat(",controller:'{0}Ctrl as vm'", item.LayoutName);
                layoutControllers.Add(item.LayoutName);

                sb.Append("});");
            }

        }
        public void CreateDirectory(string name)
        {
            if (!Directory.Exists(name))
            {
                Directory.CreateDirectory(name);
            }
        }
        private void SetNavigation(StringBuilder sb)
        {

            sb.AppendLine();
            foreach (var item in App.GetNavigation().Values)
            {
                CreateDirectory(Root + "Scripts\\Components\\" + item.WidgetName);
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
                            if (!File.Exists(PathString))
                            {
                                File.WriteAllText(PathString, "<h3>widget Name : {{vm.title}}</h3>");
                            }
                            sb.AppendFormat("templateUrl:'Scripts/Components/{0}/{0}.html'", item2.WidgetName);
                            mControllers.Add(item2.WidgetName);

                            PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}Ctrl.js", item2.WidgetName);
                            if (!File.Exists(PathString))
                            {
                                File.WriteAllText(PathString, getEmptyController(item2.WidgetName));
                            }
                            PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}Svc.js", item2.WidgetName);
                            if (!File.Exists(PathString))
                            {
                                File.WriteAllText(PathString, getEmptyService(item2.WidgetName));
                            }
                            sb.AppendFormat(",controller:'{0}Ctrl as vm'", item2.WidgetName);
                        }
                        sb.Append("}");
                        isFirst = false;
                    }
                    sb.Append("}");
                }
                if (!string.IsNullOrEmpty(item.WidgetName))
                {
                    PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}.html", item.WidgetName);
                    if (!File.Exists(PathString))
                    {
                        File.WriteAllText(PathString, "<h3>widget Name : {{vm.title}}</h3>");
                    }
                    sb.AppendFormat(",templateUrl:'Scripts/Components/{0}/{0}.html'", item.WidgetName);
                    PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}Ctrl.js", item.WidgetName);
                    if (!File.Exists(PathString))
                    {
                        File.WriteAllText(PathString, getEmptyController(item.WidgetName));
                    }
                    PathString = Root + string.Format("Scripts\\Components\\{0}\\{0}Svc.js", item.WidgetName);
                    if (!File.Exists(PathString))
                    {
                        File.WriteAllText(PathString, getEmptyService(item.WidgetName));
                    }
                    sb.AppendFormat(",controller:'{0}Ctrl as vm'", item.WidgetName);
                    mControllers.Add(item.WidgetName);
                }

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
                sb.AppendLine();
                sb.AppendFormat("import {0} from 'Scripts/{1}/{0}/{0}Ctrl.js';", item, directoryName);
            }
            list = layoutControllers.Distinct();
            directoryName = "Layouts";
            foreach (var item in list)
            {
                sb.AppendLine();
                sb.AppendFormat("import {0} from 'Scripts/{1}/{0}/{0}Ctrl.js';", item, directoryName);
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
                sb.AppendLine();
                sb.AppendFormat(".controller('{0}Ctrl', {0})", item);
            }
            list = layoutControllers.Distinct();
            foreach (var item in list)
            {
                sb.AppendLine();
                sb.AppendFormat(".controller('{0}Ctrl', {0})", item);
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
            foreach (var item in list)
            {
                if (!File.Exists(Root + string.Format("\\Scripts\\Components\\{0}\\{0}Svc.js", item))) { continue; }
                sb.AppendLine();
                sb.AppendFormat("import {0} from 'Scripts/Components/{0}/{0}Svc.js';", item);
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendFormat("var moduleName='{0}.services';", this.App.Name);
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("angular.module(moduleName,[])");
            foreach (var item in list)
            {
                if (!File.Exists(Root + string.Format("\\Scripts\\Components\\{0}\\{0}Svc.js", item))) { continue; }
                sb.AppendLine();
                sb.AppendFormat(".factory('{0}Svc', {0})", item);
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
            sb.AppendFormat("class {0}Svc", name);
            sb.AppendLine();
            sb.Append("{");
            sb.Append(Environment.NewLine + TAB1 + "constructor(){");

            sb.Append(Environment.NewLine + TAB1 + "}");
            sb.AppendLine();
            var cname = name[0].ToString().ToLower() + name.Substring(1);
            sb.AppendFormat(TAB1 + "static {0}Factory()", cname);
            sb.Append(TAB1 + "{");
            sb.AppendLine();
            sb.AppendFormat(TAB2 + "return new {0}Svc();", name);
            sb.AppendLine();
            sb.Append(TAB1 + "}");
            sb.AppendLine();
            sb.Append("}");
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
        public void CreateItem(string mode, string name)
        {
            CreateDirectory(Root + "Scripts");
            CreateDirectory(Root + "Scripts\\Components");
            CreateDirectory(Root + "Scripts\\Directives");
            try
            {
                string path = Root;
                switch (mode)
                {
                    case "Widgets":
                        path += "Scripts\\Components\\" + name;
                        CreateDirectory(path);
                        path = Root + string.Format("Scripts\\Components\\{0}\\{0}Ctrl.js", name);
                        File.WriteAllText(path, getEmptyController(name));
                        path = Root + string.Format("Scripts\\Components\\{0}\\{0}Svc.js", name);
                        File.WriteAllText(path, getEmptyService(name));
                        path = Root + string.Format("Scripts\\Components\\{0}\\{0}.html", name);
                        File.WriteAllText(path, "<h3>widget Name : {{vm.title}}</h3>");

                        break;
                    case "Components":
                        path += "Scripts\\Directives\\" + name;
                        CreateDirectory(path);
                        path = Root + string.Format("Scripts\\Directives\\{0}\\{0}.js", name);
                        File.WriteAllText(path, getEmptyDirective(name));
                        path = Root + string.Format("Scripts\\Directives\\{0}\\{0}.html", name);
                        File.WriteAllText(path, "<b>Hello world</b>");
                        path = Root + string.Format("Scripts\\Directives\\{0}\\{0}.css", name);
                        File.WriteAllText(path, "/*css goes here*/");
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }

}
