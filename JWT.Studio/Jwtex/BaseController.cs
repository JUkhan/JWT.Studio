using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Jwt.Controller
{
    public class BaseController : System.Web.Mvc.Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            SetConfig();
        }

        public string GetDefaultNavigation()
        {
            return ConfigurationManager.AppSettings["DefaultNavigation"] ?? "";
        }
        public string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public Stream StringToStream(string src)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(src);
            return new MemoryStream(byteArray);
        }
        private void SetConfig()
        {
            JwtConfig config = Session["Config"] as JwtConfig;
            if (config == null)
            {
                config = new JwtConfig();
                config.Root = Server.MapPath("~");
                if (!config.Root.EndsWith("\\"))
                {
                    config.Root += "\\";
                }
                //EntityProject
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["EntityProject"]))
                {
                    config.EntityProject = "101";
                }
                else
                {
                    string str = ConfigurationManager.AppSettings["EntityProject"];
                    config.EntityModule = str;
                    string temp = config.Root.Substring(0, config.Root.Length - 1);
                    config.EntityProject = temp.Substring(0, temp.LastIndexOf("\\") + 1) + str;
                }
                //ServiceProject
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ServiceProject"]))
                {
                    config.ServiceProject = config.Root + "Services";
                }
                else
                {
                    string str = ConfigurationManager.AppSettings["ServiceProject"];
                    string temp = config.Root.Substring(0, config.Root.Length - 1);
                    config.ServiceProject = temp.Substring(0, temp.LastIndexOf("\\") + 1) + str;
                }
                //ScriptProject
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ScriptProject"]))
                {
                    config.ScriptProject = config.Root + "Scripts";
                }
                else
                {
                    string str = ConfigurationManager.AppSettings["ScriptProject"];
                    string temp = config.Root.Substring(0, config.Root.Length - 1);
                    config.ScriptProject = temp.Substring(0, temp.LastIndexOf("\\") + 1) + str;
                }

                Session["Config"] = config;
            }
            Config = config;
        }
        public JwtConfig Config { get; set; }

    }

    public class JwtConfig
    {
        public string Root { get; set; }
        public string EntityProject { get; set; }
        public string ServiceProject { get; set; }
        public string ScriptProject { get; set; }
        public string EntityModule { get; set; }
    }
}
