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
            Design = design;
            Enabled = true;
        }
        public string Name { get; set; }
        public virtual DataTable DataSource { get; set; }
        public virtual int Layer { get; set; }
        public virtual Rect Origin { get; set; }
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
            element.Add(new XElement("Origin", Origin.X + "," + Origin.Y + "," + Origin.Height + "," + Origin.Width));
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
            var points = element.Element("Origin").Value.Split(',');
            var x = double.Parse(points[0]);
            var y = double.Parse(points[1]);
            var height = double.Parse(points[2]);
            var width = double.Parse(points[3]);
            Origin = new Rect(x, y, height, width);
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
