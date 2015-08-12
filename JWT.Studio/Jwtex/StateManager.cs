using Jwt.Controller;
using Jwtex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using log4net;
using System.Reflection;
using jwt.CodeGen;

namespace jwt.internals
{
    public class WidgetManager
    {
        public string RootPath { get; set; }
        public List<JwtWidget> WidgetList { get; set; }

        public void AddWidget(JwtWidget widget)
        {
            Deserialize();
            JwtWidget temp = WidgetList.Find(w => w.Name == widget.Name);
            if (temp != null)
            {
                WidgetList.Remove(temp);
            }
            WidgetList.Add(widget);
            Serialize();
        }
        public JwtWidget GetWidgetByName(string widgetName)
        {
            Deserialize();
            return WidgetList.Find(w => w.Name == widgetName);
        }
        public void Serialize()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<JwtWidget>));
                using (TextWriter writer = new StreamWriter(this.RootPath + @"Widget.config"))
                {
                    serializer.Serialize(writer, WidgetList);
                }
            }
            catch
            {

            }
        }
        public void Deserialize()
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<JwtWidget>));
                TextReader reader = new StreamReader(this.RootPath + @"Widget.config");
                object obj = deserializer.Deserialize(reader);
                WidgetList = (List<JwtWidget>)obj;
                reader.Close();
            }
            catch
            {
                this.WidgetList = new List<JwtWidget>();
            }

        }
    }

    public class jwtAppManager
    {
        private static object locker = new Object();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string defaultNavigation = "";
        private jwtApp app = null;
        public jwtAppManager()
        {

        }
        public jwtAppManager(string path, string defaultNavigation = "")
        {
            RootPath = path;
            this.defaultNavigation = defaultNavigation;
        }
        public string RootPath { get; set; }
        private void Serialize()
        {
            try
            {
                lock (locker)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(jwtApp));
                    using (TextWriter writer = new StreamWriter(this.RootPath + @"jwtApp.config"))
                    {
                        serializer.Serialize(writer, this.app);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }
        private void Deserialize()
        {
            try
            {
                lock (locker)
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(jwtApp));
                    TextReader reader = new StreamReader(this.RootPath + @"jwtApp.config");
                    object obj = deserializer.Deserialize(reader);
                    app = (jwtApp)obj;
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                this.app = new jwtApp();
                this.app.UILayouts = new List<Layout>();
                this.app.UINavigations = new List<Navigation>();
            }

        }

        #region Layouts
        public string AddLayout(Layout layout)
        {
            try
            {
                lock (locker)
                {
                    Deserialize();
                    var temp = app.UILayouts.Find(u => u.LayoutName == layout.LayoutName);
                    if (temp != null)
                    {
                        return string.Format("'{0}' already exist.", layout.LayoutName);
                    }
                    layout._id = Guid.NewGuid().ToString();
                    app.UILayouts.Add(layout);
                    Serialize();

                    return layout._id;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.ToString();
            }
        }
        public string UpdateLayout(Layout layout)
        {           
            try
            {
                lock (locker)
                {
                    JwtFile file = new JwtFile();
                    Deserialize();
                    var temp = app.UILayouts.Find(u => u._id == layout._id);
                    if (temp == null)
                    {
                        return string.Format("'{0}' not exist.");
                    }
                    if (!(string.IsNullOrEmpty(layout.LayoutName) || string.IsNullOrEmpty(temp.LayoutName)) && (layout.LayoutName != temp.LayoutName))
                        file.Rename(RootPath + "Scripts\\Layouts\\" + temp.LayoutName, layout.LayoutName, temp.LayoutName);
                    UpdateLayout(temp.LayoutName, layout.LayoutName);

                    temp.LayoutName = layout.LayoutName;
                    temp.Extend = layout.Extend;
                    Serialize();
                    return "Successfully Updted.";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.ToString();
            }
        }
        public void UpdateLayout(string oldName, string newName)
        {
            lock (locker)
            {
                if (oldName == newName) return;
                foreach (var item in app.UILayouts)
                {
                    if (item.Extend == oldName)
                        item.Extend = oldName;
                }
                if (app.UINavigations != null)
                    foreach (var item in app.UINavigations)
                    {
                        if (item.HasLayout == oldName)
                            item.HasLayout = newName;
                    }
            }
        }
        public string RemoveLayout(Layout layout)
        {
            try
            {
                lock (locker)
                {
                    JwtFile file = new JwtFile();
                    Deserialize();
                    var temp = app.UILayouts.Find(u => u._id == layout._id);
                    if (temp == null)
                    {
                        return string.Format("'{0}' not exist.");
                    }
                    //rename files               
                    file.Remove(RootPath + "Scripts\\Layouts\\" + temp.LayoutName);

                    app.UILayouts.Remove(temp);
                    Serialize();

                    return "Successfully Removed.";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.ToString();
            }
        }
        public List<Layout> GetLayoutList()
        {
            try
            {
                lock (locker)
                {
                    Deserialize();
                    return app.UILayouts ?? new List<Layout>();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new List<Layout>();
            }
        }
        #endregion

        #region Navigations
        public string AddNavigation(Navigation navigation)
        {
            try
            {
                lock (locker)
                {
                    Deserialize();
                    var temp = app.UINavigations.Find(u => u.NavigationName == navigation.NavigationName);
                    if (temp != null)
                    {
                        return string.Format("'{0}' already exist.");
                    }
                    navigation._id = Guid.NewGuid().ToString();
                    app.UINavigations.Add(navigation);
                    Serialize();

                    return navigation._id;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.ToString();
            }
        }
        public string UpdateNavigation(Navigation navigation)
        {
            log.Info(navigation.NavigationName);
            JwtFile file = new JwtFile();
            try
            {
                lock (locker)
                {
                    Deserialize();
                    var temp = app.UINavigations.Find(u => u._id == navigation._id);
                    if (temp == null)
                    {
                        return string.Format("'{0}' not exist.");
                    }
                    if (!(string.IsNullOrEmpty(navigation.NavigationName) || string.IsNullOrEmpty(temp.NavigationName)) && (navigation.NavigationName != temp.NavigationName))
                        file.Rename(RootPath + "Scripts\\Components\\" + temp.WidgetName, navigation.WidgetName, temp.WidgetName);
                    UpdateNavigation(temp.WidgetName, navigation.WidgetName);

                    temp.NavigationName = navigation.NavigationName;
                    temp.WidgetName = navigation.WidgetName;
                    temp.ParamName = navigation.ParamName;
                    temp.UIViews = navigation.UIViews;
                    temp.HasLayout = navigation.HasLayout;
                    Serialize();

                    return "Successfully Updted.";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.ToString();
            }
        }

        private void UpdateNavigation(string oldName, string newName)
        {
            if (oldName == newName) return;
            foreach (var item in app.UINavigations)
            {
                if (item.WidgetName == oldName)
                    item.WidgetName = newName;
                if (item.UIViews != null)
                {
                    foreach (var view in item.UIViews)
                    {
                        if (view.WidgetName == oldName)
                            view.WidgetName = newName;
                    }
                }
            }
        }
        public string RemoveNavigation(Navigation navigation)
        {
            try
            {
                lock (locker)
                {
                    Deserialize();
                    var temp = app.UINavigations.Find(u => u._id == navigation._id);
                    if (temp == null)
                    {
                        return string.Format("'{0}' not exist.");
                    }
                    //remove files               
                    //Remove(RootPath + "Scripts\\Components\\" + temp.WidgetName);

                    app.UINavigations.Remove(temp);
                    Serialize();

                    return "Successfully Removed.";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.ToString();
            }
        }
        public List<Navigation> GetNavigationList()
        {
            try
            {
                lock (locker)
                {
                    Deserialize();
                    return app.UINavigations ?? new List<Navigation>();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new List<Navigation>();
            }
        }
        #endregion

        public void GenerateConfig()
        {
            try
            {
                lock (locker)
                {
                    Deserialize();
                    Jwtex.CodeGen codeGen = new Jwtex.CodeGen();
                    codeGen.App = this.app;
                    codeGen.Root = RootPath;
                    codeGen.DefaultNavigation = defaultNavigation;
                    foreach (var item in app.UILayouts)
                    {
                        app.Layout = item;
                    }
                    foreach (var item in app.UINavigations)
                    {
                        foreach (var view in item.UIViews)
                        {
                            item.View = view;
                        }
                        app.Navigation = item;
                    }
                    codeGen.Execute();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }

        
    }

}
