using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
namespace Jwt.Resource
{
    public sealed class JwtResourceRouteHandler : IRouteHandler
    {
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return new EmbeddedResourceHttpHandler(requestContext.RouteData);
        }

        public class EmbeddedResourceHttpHandler : IHttpHandler
        {
            private RouteData _RouteData;
            public EmbeddedResourceHttpHandler(RouteData routeData)
            {
                _RouteData = routeData;
            }

            public bool IsReusable
            {
                get { return false; }
            }

            public void ProcessRequest(HttpContext context)
            {
                string group = _RouteData.Values["group"].ToString();
                string fileName = _RouteData.Values["file"].ToString();
                string fileExtension = _RouteData.Values["extension"].ToString();
                if (!Regex.IsMatch(fileExtension, ("js|html|png")))
                    throw new InvalidOperationException("Request not valid");

                string nameSpace = GetType().Assembly.GetName().Name;
                string manifestResourceName = string.Format("{0}.{1}.{2}.{3}",
                    nameSpace, group, fileName, fileExtension);

                using (Stream stream = GetType().Assembly.GetManifestResourceStream(manifestResourceName))
                {
                    context.Response.Clear();
                    if (fileExtension == "js")
                        context.Response.ContentType = "text/javascript";
                    else if (fileExtension == "css")
                        context.Response.ContentType = "text/css";
                    else if (fileExtension == "png")
                        context.Response.ContentType = "image/png";

                    stream.CopyTo(context.Response.OutputStream);
                }
            }
        }
    }
}
