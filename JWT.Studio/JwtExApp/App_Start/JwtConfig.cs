using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jwtex;
namespace JwtExApp
{
    public class JwtConfig
    {
        public static void RegisterComponent()
        {
            Fluently.With(new jwtApp())
                .Do(app => app.Layout = new Layout { LayoutName = "layout1" })
                .Do(app => app.Navigation = new Navigation { NavigationName = "nav1", WidgetName = "DbWeather" })
                .Do(app => app.Navigation = new Navigation { NavigationName = "nav2", WidgetName = "testWidget" })
                .Do(app => app.Navigation = Fluently.With(new Navigation { NavigationName = "nav3",  HasLayout = "layout1" })
                    .Do(nav => nav.View = new View { ViewName = "view1", WidgetName = "DbWeather" })
                     .Do(nav => nav.View = new View { ViewName = "view2", WidgetName = "testWidget" })
                    .Done())
                .Done()
                .Execute();
        }
    }
}