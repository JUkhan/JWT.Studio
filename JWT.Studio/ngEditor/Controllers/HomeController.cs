using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using ngEditor.Models;
using System.IO.Compression;

namespace ngEditor.Controllers
{
    public class HomeController : System.Web.Mvc.Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.Root = Server.MapPath("~");
            if (!Root.EndsWith("\\"))
            {
                Root += "\\";
            }
        }
        public string Root { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public void ExtractContent(Project project)
        {
            try
            {
                using (FileStream fs = System.IO.File.OpenRead(this.Root + "basket.zip"))

                using (MemoryStream mem = new MemoryStream())
                {
                    fs.CopyTo(mem);
                    using (ZipArchive arc = new ZipArchive(mem))
                    {
                        arc.ExtractToDirectory(project.path);

                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult AddProject(Project project)
        {
            try
            {
                //if (!(project.path.EndsWith("/") || project.path.EndsWith(@"\")))
                //{
                //    project.path += '/';
                //}
                //project.path += project.name;
                var file = new jwt.CodeGen.JwtFile();
                if (!file.DirectoryExists(project.path))
                {
                    file.CreateDirectory(project.path);
                }
                
                Deserialize();
                var item = _projects.ProjectList.FirstOrDefault(x => x.name == project.name);
                if (item != null)
                {
                    return Json("Project already exist.");
                }
                _projects.ProjectList.Add(project);
                Serialize();
                if (project.allowTemplate)
                    ExtractContent(project);
                else if(!file.DirectoryExists(project.path+"/Scripts"))
                {
                    file.CreateDirectory(project.path + "/Scripts");
                }
                else if (!file.DirectoryExists(project.path + "/Content"))
                {
                    file.CreateDirectory(project.path + "/Content");
                }
                else if (!file.DirectoryExists(project.path + "/Scripts/Layouts"))
                {
                    file.CreateDirectory(project.path + "/Scripts/Layouts");
                }
                return Json("101");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        public JsonResult GetProjects()
        {
            Deserialize();

            return Json(_projects.ProjectList, JsonRequestBehavior.AllowGet);
        }
        public ProjectConfig _projects { get; set; }
        private void Serialize()
        {
            try
            {

                XmlSerializer serializer = new XmlSerializer(typeof(ProjectConfig));
                using (TextWriter writer = new StreamWriter(this.Root + @"editor.config"))
                {
                    serializer.Serialize(writer, this._projects);
                }

            }
            catch (Exception ex)
            {
               
            }
        }

        public JsonResult LoadProject(Project project)
        {
            Session["ROOT_PATH"] = project.path;
            return Json("sas");
        }
        private void Deserialize()
        {
            try
            {

                XmlSerializer deserializer = new XmlSerializer(typeof(ProjectConfig));
                using (TextReader reader = new StreamReader(this.Root + @"editor.config"))
                {
                    object obj = deserializer.Deserialize(reader);
                    _projects = (ProjectConfig)obj;
                }

            }
            catch (Exception ex)
            {
                _projects = new ProjectConfig();
            }

        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}