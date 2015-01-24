using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
namespace Jwtex
{
    public class App
    {
        public string Name { get; set; }

        public App()
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
            Console.WriteLine("Executed successfully" + _navigations.Count);
        }
    }

    public class Layout
    {
        public string LayoutName { get; set; }
        public string Extend { get; set; }

    }
    public class Navigation
    {
        public string NavigationName { get; set; }
        public string HasLayout { get; set; }
        public string ParamName { get; set; }
        public string WidgetName { get; set; }
        private Dictionary<string, View> _views = new Dictionary<string, View>();

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
        public CodeGen(App app)
        {
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            this.App = app;
            this.Root = AppDomain.CurrentDomain.BaseDirectory;
        }
        public App App { get; set; }
        public void Execute()
        {
            StringBuilder sb = new StringBuilder();
            //stateprovider.state('Student',{url:'/Student',templateUrl:root + 'Templates/Components/Student.html',controller:'StudentCtrl'});
            sb.Append("angular.module('" + App.Name + "').config(['$stateProvider', '$urlRouterProvider', function (stateprovider, routeProvider) {");
            SetLayout(sb);
            SetNavigation(sb);
            sb.AppendLine();
            sb.Append("}]);");
            sb.AppendLine();
            sb.Append("jwt._arr={");
            GetNamArr(sb);
            sb.Append("};");
            sb.AppendLine();
            System.IO.File.WriteAllText(this.Root + "Scripts\\router.js", sb.ToString());


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

            sb.AppendLine();

            foreach (Layout item in App.GetLayout().Values)
            {
                sb.AppendLine();
                sb.Append(TAB1);
                sb.AppendFormat("stateprovider.state('{0}'", GetStateName(item));
                sb.Append(",{abstract:true,");
                sb.AppendFormat(@"url:'/{0}'", item.LayoutName);
                sb.AppendFormat(",templateUrl:'Templates/Layouts/{0}.html'", item.LayoutName);
                PathString = Root + "Scripts\\Controllers\\" + item.LayoutName + "Ctrl.js";

                if (File.Exists(PathString))
                {
                    sb.AppendFormat(",controller:'{0}Ctrl'", item.LayoutName);
                }
                sb.Append("});");
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
                            sb.AppendFormat("templateUrl:'Templates/Widgets/{0}/{0}.html'", item2.WidgetName);
                        PathString = Root + "Scripts\\Controllers\\" + item2.WidgetName + "Ctrl.js";
                        if (File.Exists(PathString))
                        {
                            sb.AppendFormat(",controller:'{0}Ctrl'", item2.WidgetName);
                        }
                        sb.Append("}");
                        isFirst = false;
                    }
                    sb.Append("}");
                }
                else
                {
                    sb.AppendFormat(",templateUrl:'Templates/Widgets/{0}/{0}.html'", item.WidgetName??"temp");
                }
                PathString = Root + "Scripts\\Controllers\\"+ item.WidgetName + "Ctrl.js";
                if (File.Exists(PathString))
                {
                    sb.AppendFormat(",controller:'{0}Ctrl'", item.WidgetName);
                }
                sb.Append("});");
            }
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
