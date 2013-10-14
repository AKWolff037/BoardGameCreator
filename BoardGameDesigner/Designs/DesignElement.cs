using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Media;
using BoardGameDesigner.IO;
namespace BoardGameDesigner.Designs
{
    public abstract class DesignElement : IDesignElement
    {
        internal DesignElement(IDesign design)
        {
            Name = "New Design Element";
            Design = design;
            Enabled = true;
            Size = new Size(30, 30);
            X_Offset = 0;
            Y_Offset = 0;
        }
        public string Name { get; set; }
        public virtual DataTable DataSource { get; set; }
        public virtual int Layer { get; set; }
        public virtual double X_Offset { get; set; }
        public virtual double Y_Offset { get; set; }
        public virtual Size Size { get; set; }
        public virtual IDesign Design { get; set; }
        public virtual ICondition Condition { get; set; }
        public virtual DataColumn ValueSource { get; set; }
        public bool Enabled { get; set; } 
        public abstract void Draw(DrawingContext context);
        public abstract XElement ToXmlElement();        
        public abstract IXmlElementConvertible FromXmlElement(XElement element);
        public void AddBasePropertiesToXmlElement(XElement element)
        {
            element.Add(new XElement("Name", Name));
            if (Condition != null)
            {
                element.Add(Condition.ToXmlElement());
            }
            if (DataSource != null)
            {
                element.Add(new XElement("DataSource", DataSource.TableName));
            }
            if (ValueSource != null)
            {
                element.Add(new XElement("ValueSource", Data.DataSetConverter.ConvertDataColumnToXmlElement(ValueSource)));
            }
            element.Add(new XElement("Layer", Layer));
            element.Add(new XElement("Enabled", Enabled));
            element.Add(new XElement("X_Offset", X_Offset));
            element.Add(new XElement("Y_Offset", Y_Offset));
            element.Add(new XElement("Size", new XElement("Width", Size.Width), new XElement("Height", Size.Height)));
        }
        public void LoadBasePropertiesFromXmlElement(XElement element)
        {
            var dataset = this.Design.DesignManager.Dataset;
            Name = element.Element("Name").Value;
            if (element.Element("DataSource") != null)
            {
                DataSource = dataset.Tables[element.Element("DataSource").Value];
            }
            if (element.Element("ValueSource") != null)
            {
                ValueSource = Data.DataSetConverter.ConvertDataColumnFromXmlElement(element.Element("ValueSource").Element("Column"));
            }
            if (ValueSource != null && DataSource != null)
            {
                ValueSource = DataSource.Columns[ValueSource.ColumnName];
            }
            Layer = int.Parse(element.Element("Layer").Value);
            Enabled = bool.Parse(element.Element("Enabled").Value);
            X_Offset = double.Parse(element.Element("X_Offset").Value);
            Y_Offset = double.Parse(element.Element("Y_Offset").Value);
            Size = new Size(double.Parse(element.Element("Size").Element("Width").Value), double.Parse(element.Element("Size").Element("Height").Value));
            Condition = Designs.Condition.ParseElement(this, element);
        }
        public void Remove()
        {
            this.Condition = null;
            this.ValueSource = null;
            this.Layer = -1;
            this.DataSource = null;
            this.Design.DesignElements.Remove(this);
        }
    }
}
