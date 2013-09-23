using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml.Linq;
using BoardGameDesigner.IO;
using BoardGameDesigner.Projects;
namespace BoardGameDesigner.Designs
{
    public class DesignManager : IDesignManager
    {
        public List<IDesign> Designs { get; private set; }
        public DataSet Dataset { get; set; }
        public IProject Project { get; set; }
        public DesignManager(IProject project)
        {
            Project = project;
            Designs = new List<IDesign>();
            Dataset = new DataSet("New Dataset");
        }
        public XElement ToXmlElement()
        {
            var xElement = new XElement("DesignManager");
            foreach (IDesign design in Designs)
            {
                var designElem = design.ToXmlElement();
                xElement.Add(designElem);
            }
            xElement.Add(BoardGameDesigner.Data.DataSetConverter.ConvertToXmlElement(Dataset));
            return xElement;
        }
        public IXmlElementConvertible FromXmlElement(XElement element)
        {
            Dataset = BoardGameDesigner.Data.DataSetConverter.ConvertFromXmlElement(element.Element("DataSet"));
            foreach (XElement designElem in element.Elements("Design"))
            {
                var design = new Design(this).FromXmlElement(designElem) as Design;
                Designs.Add(design);
            }
            return this;
        }
    }
}
