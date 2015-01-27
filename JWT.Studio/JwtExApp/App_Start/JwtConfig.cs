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
                .Do(app => app.Navigation = new Navigation { NavigationName = "nav1", WidgetName = "DbWeather" })
                .Done()
                .Execute();
        }
    }
}