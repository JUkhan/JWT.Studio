using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
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
        public jwtApp App { get; set; }
        public void Execute()
        {
            CreateDirectory(Root + "Scripts");
            CreateDirectory(Root + "Scripts\\Controllers");
            CreateDirectory(Root + "Scripts\\Services");
            CreateDirectory(Root + "Templates");
            CreateDirectory(Root + "Templates\\Widgets");
            CreateDirectory(Root + "Templates\\Layouts");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.Append("export default function config(stateprovider, routeProvider){");           
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
                sb.AppendLine();
                sb.Append(TAB1);
                sb.AppendFormat("stateprovider.state('{0}'", GetStateName(item));
                sb.Append(",{abstract:true,");
                sb.AppendFormat(@"url:'/{0}'", item.LayoutName);                

                PathString = Root + string.Format("Templates\\Layouts\\{0}.html", item.LayoutName);
                if (!File.Exists(PathString))
                {
                    File.WriteAllText(PathString, string.Format("<h3>Layout Name : {0}</h3><div ui-view></div>", item.LayoutName));
                }
                sb.AppendFormat(",templateUrl:'Templates/Layouts/{0}.html'", item.LayoutName);               

               
                PathString = Root + "Scripts\\Controllers\\" + item.LayoutName + "Ctrl.js";
                if (!File.Exists(PathString))
                {
                    File.WriteAllText(PathString, getEmptyController(item.LayoutName));
                }
                sb.AppendFormat(",controller:'{0}Ctrl as vm'", item.LayoutName);
                mControllers.Add(item.LayoutName);
                
                sb.Append("});");
            }

        }
        private void CreateDirectory(string name)
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
                sb.AppendLine();
                sb.Append(TAB1);
                sb.AppendFormat("stateprovider.state('{0}'", GetStateName(item));
                sb.Append(",{");
                sb.AppendFormat(@"url:'/{0}{1}'", item.NavigationName, string.IsNullOrEmpty(item.ParamName) ? "" : "/:" + item.ParamName);

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
                            PathString = Root + string.Format("Templates\\Widgets\\{0}.html", item2.WidgetName);
                            if (!File.Exists(PathString))
                            {
                                File.WriteAllText(PathString, "<h3>widget Name : {{vm.title}}</h3>");
                            }                            
                            sb.AppendFormat("templateUrl:'Templates/Widgets/{0}.html'", item2.WidgetName);
                            mControllers.Add(item2.WidgetName );
                        }
                        PathString = Root + "Scripts\\Controllers\\" + item2.WidgetName + "Ctrl.js";
                        if (!File.Exists(PathString))
                        {
                            File.WriteAllText(PathString, getEmptyController(item2.WidgetName));
                        }
                        sb.AppendFormat(",controller:'{0}Ctrl as vm'", item2.WidgetName);
                        sb.Append("}");
                        isFirst = false;
                    }
                    sb.Append("}");
                }
                if (!string.IsNullOrEmpty(item.WidgetName))
                {                    
                    PathString = Root + string.Format("Templates\\Widgets\\{0}.html", item.WidgetName);
                    if (!File.Exists(PathString))
                    {
                        File.WriteAllText(PathString, "<h3>widget Name : {{vm.title}}</h3>");
                    }                    
                    sb.AppendFormat(",templateUrl:'Templates/Widgets/{0}.html'", item.WidgetName);
                    PathString = Root + "Scripts\\Controllers\\" + item.WidgetName + "Ctrl.js";
                    if (!File.Exists(PathString))
                    {
                        File.WriteAllText(PathString, getEmptyController(item.WidgetName));
                    }
                    sb.AppendFormat(",controller:'{0}Ctrl as vm'", item.WidgetName);
                    mControllers.Add(item.WidgetName);
                }

                sb.Append("});");
            }
           
        }

        private void GenAllControllers()
        {
            var list = mControllers.Distinct();
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {                
                sb.AppendLine();
                sb.AppendFormat("import {0} from 'Scripts/Controllers/{0}.js';", item+"Ctrl");
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendFormat("var moduleName='{0}.controllers';", this.App.Name);
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("angular.module(moduleName,[])");
            foreach (var item in list)
            {
                sb.AppendLine();
                sb.AppendFormat(".controller('{0}', {0})", item+"Ctrl");
            }
            sb.Append(";");
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("export default moduleName;");
            File.WriteAllText(Root+"Scripts\\app.controllers.js", sb.ToString());
        }
        private void GenAllServices()
        {
            var list = mControllers.Distinct();
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                if (!File.Exists(Root + "\\Scripts\\Services\\" + item + "Svc.js")) { continue; }
                sb.AppendLine();
                sb.AppendFormat("import {0} from 'Scripts/Services/{0}.js';", item + "Svc");
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendFormat("var moduleName='{0}.services';", this.App.Name);
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("angular.module(moduleName,[])");
            foreach (var item in list)
            {
                if (!File.Exists(Root + "\\Scripts\\Services\\" + item + "Svc.js")) { continue; }
                sb.AppendLine();
                sb.AppendFormat(".factory('{0}', {0})", item+"Svc");
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
    }

}
