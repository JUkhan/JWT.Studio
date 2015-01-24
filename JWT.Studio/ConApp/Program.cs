using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jwtex;


namespace ConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Fluently.With(new jwtApp())
                .Do(app => app.Layout = new Layout { LayoutName = "layout1" })
                .Do(app => app.Layout = new Layout { LayoutName = "layout2", Extend="layout1" })
                .Do(app => app.Navigation = new Navigation { NavigationName="nav1", HasLayout="layout1", Widget="Student" })
                .Do(app => app.Navigation = Fluently.With(new Navigation { NavigationName = "nav2", HasLayout = "layout2" })
                    .Do(nav => nav.View = new View { ViewName="box1", WidgetName="Student" })
                    .Do(nav => nav.View = new View {  ViewName="box2", WidgetName="Department" })
                    .Done())              
                .Done()
                .Execute();
            Console.ReadKey();
        }
    }
}
