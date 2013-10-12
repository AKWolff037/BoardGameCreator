using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Xml.Linq;
using BoardGameDesigner.IO;
namespace BoardGameDesigner.Designs
{
    public class Design : IDesign
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public List<IDesignElement> DesignElements { get; private set; }
        public BitmapImage Template { get; set; }
        public IDesignManager DesignManager { get; set; }
        public Design(IDesignManager manager)
        {
            Initialize("New Design", new List<IDesignElement>(), null, manager);
        }
        public Design(string name, IDesignManager manager)
        {
            Initialize(name, new List<IDesignElement>(), null, manager);
        }
        public Design(string name, List<IDesignElement> elements, IDesignManager manager)
        {
            Initialize(name, elements, null, manager);
        }
        public Design(string name, List<IDesignElement> elements, BitmapImage template, IDesignManager manager)
        {
            Initialize(name, elements, template, manager);
        }

        private void Initialize(string name, List<IDesignElement> elements, BitmapImage template, IDesignManager manager)
        {
            Name = name;
            DesignElements = elements;
            Template = template;
            DesignManager = manager;
            Enabled = true;
        }
        public void Remove()
        {
            foreach (var elem in DesignElements)
            {
                elem.Remove();
            }
            Template = null;
            this.DesignManager.Designs.Remove(this);            
        }
        public RenderTargetBitmap DrawImage()
        {
            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            if (Template != null)
            {
                drawingContext.DrawImage(Template, new Rect(0, 0, Template.Width, Template.Height));
                var sortedElements = DesignElements.Where(elem => elem.Enabled).OrderBy(elem => elem.Layer);
                foreach (var elem in sortedElements)
                {
                    elem.Draw(drawingContext);
                }
                drawingContext.Close();

                var render = new RenderTargetBitmap(Template.PixelWidth, Template.PixelHeight, 300, 300, PixelFormats.Default);
                render.Render(drawingVisual);                
                return render;
            }
            else
            {
                return null;
            }
        }
        public void AddDesignElement(IDesignElement element)
        {
            element.Design = this;
            if (!DesignElements.Contains(element))
            {
                DesignElements.Add(element);
            }
        }
        public XElement ToXmlElement()
        {
            var xElement = new XElement("Design",
                    new XElement("Name", Name),
                    new XElement("Template", Template.UriSource),
                    new XElement("Enabled", Enabled));
            var designElementsElement = new XElement("DesignElements");
            foreach(IDesignElement desElem in DesignElements)
            {
                designElementsElement.Add(desElem.ToXmlElement());
            }
            xElement.Add(designElementsElement);
            //TODO
            return xElement;
        }
        public IXmlElementConvertible FromXmlElement(XElement element)
        {
            Name = element.Element("Name").Value;
            Template = new BitmapImage(new Uri(element.Element("Template").Value));
            Enabled = bool.Parse(element.Element("Enabled").Value);
            foreach (XElement designElem in element.Element("DesignElements").Elements("TextDesignElement"))
            {
                var textElement = new TextDesignElement(this).FromXmlElement(designElem) as TextDesignElement;
                DesignElements.Add(textElement);
            }
            foreach (XElement designElem in element.Element("DesignElements").Elements("ImageDesignElement"))
            {
                var imgElement = new ImageDesignElement(this).FromXmlElement(designElem) as ImageDesignElement;
                DesignElements.Add(imgElement);
            }
            return this;
        }

    }
}
