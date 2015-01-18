using Jwt.Controller;
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
    public class StateManager
    {
        public string RootPath { get; set; }
        public List<State> StateList { get; set; }

        public string AddState(State state)
        {
            state.Id = Guid.NewGuid().ToString();
            State temp = StateList.FirstOrDefault(x => x.StateName == state.StateName);
            if (temp == null)
            {
                StateList.Add(state);

                return state.Id;
            }
            return "Already Exist";
        }
        public string UpdateState(State state)
        {
            State temp = StateList.FirstOrDefault(x => x.Id == state.Id);
            if (temp != null)
            {
                temp.StateName = state.StateName;
                temp.IsAbstract = state.IsAbstract;
                temp.Url = state.Url;
                temp.StateController = state.StateController;
                temp.TemplateUrl = state.TemplateUrl;
                temp.StateViews = state.StateViews;
                temp.Parent = state.Parent;
                return "State updated successfully";
            }
            return "Not Found";
        }
        public string RemoveState(State state)
        {
            State temp = StateList.FirstOrDefault(x => x.Id == state.Id);
            if (temp != null)
            {
                StateList.Remove(temp);
                return "State removed successfully";
            }
            return "Not Found";
        }
        public void Serialize()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<State>));
                using (TextWriter writer = new StreamWriter(this.RootPath + @"State.config"))
                {
                    serializer.Serialize(writer, StateList);
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
                XmlSerializer deserializer = new XmlSerializer(typeof(List<State>));
                TextReader reader = new StreamReader(this.RootPath + @"State.config");
                object obj = deserializer.Deserialize(reader);
                StateList = (List<State>)obj;
                reader.Close();
            }
            catch
            {
                this.StateList = new List<State>();
            }

        }

    }

    public class State
    {
        public State()
        {
            StateViews = new List<StateView>();
        }
        public string Id { get; set; }
        public string StateName { get; set; }
        public string Url { get; set; }
        public string StateController { get; set; }
        public string TemplateUrl { get; set; }
        public bool IsAbstract { get; set; }
        public string Parent { get; set; }
        public List<StateView> StateViews { get; set; }
    }
    public class StateView
    {
        public string TemplateUrl { get; set; }
        public string ControllerName { get; set; }
        public string ViewName { get; set; }
    }
}
