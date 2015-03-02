﻿using Jwt.Controller;
using Jwtex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
        private jwtApp app = null;
        public jwtAppManager()
        {

        }
        public jwtAppManager(string path)
        {
            RootPath = path;
        }
        public string RootPath { get; set; }
        private void Serialize()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(jwtApp));
                using (TextWriter writer = new StreamWriter(this.RootPath + @"jwtApp.config"))
                {
                    serializer.Serialize(writer, this.app);
                }
            }
            catch
            {

            }
        }
        private void Deserialize()
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(jwtApp));
                TextReader reader = new StreamReader(this.RootPath + @"jwtApp.config");
                object obj = deserializer.Deserialize(reader);
                app = (jwtApp)obj;
                reader.Close();
            }
            catch
            {
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
                Deserialize();
                var temp = app.UILayouts.Find(u => u.LayoutName == layout.LayoutName);
                if (temp != null)
                {
                    return string.Format("'{0}' already exist.");
                }
                layout._id = Guid.NewGuid().ToString();
                app.UILayouts.Add(layout);
                Serialize();

                return layout._id;
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        public string UpdateLayout(Layout layout)
        {
            try
            {
                Deserialize();
                var temp = app.UILayouts.Find(u => u._id == layout._id);
                if (temp == null)
                {
                    return string.Format("'{0}' not exist.");
                }
                UpdateLayout(temp.LayoutName, layout.LayoutName);
                //rename files
                var preControllerName = RootPath + "Scripts\\Controllers\\" + temp.LayoutName + "Ctrl.js";
                var newControllerName = RootPath + "Scripts\\Controllers\\" + layout.LayoutName + "Ctrl.js";
                RenameFile(preControllerName, newControllerName);
                var preTemplateName = RootPath + "Templates\\Layouts\\" + temp.LayoutName + ".html";
                var newTemplateName = RootPath + "Templates\\Layouts\\" + layout.LayoutName + ".html";
                RenameFile(preTemplateName, newTemplateName);

                temp.LayoutName = layout.LayoutName;
                temp.Extend = layout.Extend;
                Serialize();
                return "Successfully Updted.";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        public void UpdateLayout(string oldName, string newName)
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
        public string RemoveLayout(Layout layout)
        {
            try
            {
                Deserialize();
                var temp = app.UILayouts.Find(u => u._id == layout._id);
                if (temp == null)
                {
                    return string.Format("'{0}' not exist.");
                }
                //rename files               
                RemoveFile(RootPath + "Scripts\\Controllers\\" + temp.LayoutName + "Ctrl.js");
                RemoveFile(RootPath + "Templates\\Layouts\\" + temp.LayoutName + ".html");

                app.UILayouts.Remove(temp);
                Serialize();

                return "Successfully Removed.";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        public List<Layout> GetLayoutList()
        {
            try
            {
                Deserialize();
                return app.UILayouts ?? new List<Layout>();
            }
            catch (Exception ex)
            {

                return new List<Layout>();
            }
        }
        #endregion

        #region Navigations
        public string AddNavigation(Navigation navigation)
        {
            try
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
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        public string UpdateNavigation(Navigation navigation)
        {
            try
            {
                Deserialize();
                var temp = app.UINavigations.Find(u => u._id == navigation._id);
                if (temp == null)
                {
                    return string.Format("'{0}' not exist.");
                }
                UpdateNavigation(temp.WidgetName, navigation.WidgetName);
                //rename files
                var preControllerName = RootPath + "Scripts\\Controllers\\" + temp.WidgetName + "Ctrl.js";
                var newControllerName = RootPath + "Scripts\\Controllers\\" + navigation.WidgetName + "Ctrl.js";
                RenameFile(preControllerName, newControllerName);
                var preTemplateName = RootPath + "Templates\\Widgets\\" + temp.WidgetName + ".html";
                var newTemplateName = RootPath + "Templates\\Widgets\\" + navigation.WidgetName + ".html";
                RenameFile(preTemplateName, newTemplateName);

                temp.NavigationName = navigation.NavigationName;
                temp.WidgetName = navigation.WidgetName;
                temp.ParamName = navigation.ParamName;
                temp.UIViews = navigation.UIViews;
                temp.HasLayout = navigation.HasLayout;
                Serialize();

                return "Successfully Updted.";
            }
            catch (Exception ex)
            {

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
                Deserialize();
                var temp = app.UINavigations.Find(u => u._id == navigation._id);
                if (temp == null)
                {
                    return string.Format("'{0}' not exist.");
                }
                //rename files               
                RemoveFile(RootPath + "Scripts\\Controllers\\" + temp.WidgetName + "Ctrl.js");
                RemoveFile(RootPath + "Templates\\Widgets\\" + temp.WidgetName + ".html");

                app.UINavigations.Remove(temp);
                Serialize();

                return "Successfully Removed.";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        public List<Navigation> GetNavigationList()
        {
            try
            {
                Deserialize();
                return app.UINavigations ?? new List<Navigation>();
            }
            catch (Exception ex)
            {

                return new List<Navigation>();
            }
        }
        #endregion

        public void GenerateConfig()
        {
            try
            {
                Deserialize();
                Jwtex.CodeGen codeGen = new Jwtex.CodeGen();
                codeGen.App = this.app;
                codeGen.Root = RootPath;
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
            catch (Exception)
            {

                throw;
            }

        }

        #region File Operations
        private bool IsExist(string path)
        {
            return System.IO.File.Exists(path);
        }
        private void RenameFile(string previousName, string newName)
        {
            if (previousName == newName) return;
            if (IsExist(previousName))
            {
                System.IO.File.Move(previousName, newName);
            }
        }
        private void RemoveFile(string fileName)
        {
            if (IsExist(fileName))
            {
                System.IO.File.Delete(fileName);
            }
        }
        #endregion
    }

}
