using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Jwt.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace jwtApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();
            setLog4Net();
           // JwtConfig.RegisterComponent();
        }
        private void setLog4Net()
        {
            Fluently.With(LogManager.GetRepository() as Hierarchy)
               .Do(repo => repo.Root.AddAppender(
                   Fluently.With(new RollingFileAppender())
                   .Do(appender => appender.File = "logfile.txt")
                   .Do(appender => appender.AppendToFile = true)
                   .Do(appender => appender.StaticLogFileName = true)
                   .Do(appender => appender.MaxSizeRollBackups = 10)
                   .Do(appender => appender.MaximumFileSize = "10MB")
                   .Do(appender => appender.RollingStyle = RollingFileAppender.RollingMode.Size)
                   .Do(appender => appender.Layout =
                       Fluently.With(new PatternLayout())
                       .Do(layout => layout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline")
                       .Do(layout => layout.ActivateOptions())
                       .Done())
                   .Do(appender => appender.ActivateOptions())
                   .Done()))
               .Do(repo => repo.Root.AddAppender(
                   Fluently.With(new MemoryAppender())
                   .Do(memory => memory.ActivateOptions())
                   .Done()))
               .Do(repo => repo.Root.Level = Level.Info)
               .Do(repo => repo.Configured = true)
              .Done();
        }
    }
}
