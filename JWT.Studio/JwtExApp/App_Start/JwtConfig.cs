using Jwtex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JwtExApp
{
    public static class JwtConfig
    {
        public static void RegisterComponent()
        {
            Fluently.With(new App())
               .Do(app => app.Layout = new Layout { LayoutName="layout1" })                
               .Do(app => app.Navigation =
                     Fluently.With(new Navigation { NavigationName = "nav1", HasLayout="layout1" })
                     .Do(nav => nav.View = new View {  ViewName="col1", WidgetName="widget1"})
                     .Do(nav => nav.View = new View { ViewName = "col2", WidgetName = "widget1" })
                     .Done())                   
               .Done()
               .Execute();
        }
    }
}