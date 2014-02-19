using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BoardGameDesigner.IO;
using BoardGameDesigner.Designs;
using System.Windows;
namespace BoardGameDesigner.Projects
{
    public class GameProject : IProject
    {
        public virtual string Name { get; set; }
        public virtual IDesignManager DesignManager { get; set;}
        public bool IsDirty { get; set; }
        public virtual string ProjectFilePath { get; set; } 
        public virtual event RoutedEventHandler Saved;
        public virtual void Save()
        {
            IO.ProjectIOManager.SaveProject(this);
            if (Saved != null)
            {
                var args = new RoutedEventArgs(null);
                Saved(this, args);
            }
        }
        public GameProject()
        {
            Name = "New Project";
            DesignManager = new Designs.DesignManager(this);
        }
        public GameProject(string name)
        {
            Name = name;
            DesignManager = new Designs.DesignManager(this);
        }
        public XElement ToXmlElement()
        {
            var projectNode = new XElement("Project",
                new XElement("Name", Name),
                    DesignManager.ToXmlElement()
                );
            return projectNode;
        }
        public IXmlElementConvertible FromXmlElement(System.Xml.Linq.XElement element)
        {
            Name = element.Element("Name").Value;
            DesignManager = new DesignManager(this).FromXmlElement(element.Element("DesignManager")) as IDesignManager;
            return this;
        }
    }
}
