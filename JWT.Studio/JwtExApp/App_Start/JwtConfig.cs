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
            Fluently.With(new jwtApp())
               .Do(app => app.Layout = new Layout { LayoutName = "layout1" })
               .Do(app => app.Layout = new Layout { LayoutName = "layout2" })
               .Do(app => app.Navigation =
                     Fluently.With(new Navigation { NavigationName = "nav1", HasLayout = "layout1" })
                     .Do(nav => nav.View = new View { ViewName = "col1", WidgetName = "widget1" })
                     .Do(nav => nav.View = new View { ViewName = "col2", WidgetName = "widget2" })
                     .Done())
                .Do(app => app.Navigation = new Navigation { NavigationName = "nav2", WidgetName = "widget3", HasLayout = "layout2" })
               .Done()
               .Execute();
        }
    }
}