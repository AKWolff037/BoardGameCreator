using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BoardGameDesigner.IO;
namespace BoardGameDesigner.Designs
{
    public class ImageDesignElement : DesignElement, IImageDesignElement
    {
        public BitmapImage Image { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public ImageDesignElement(IDesign design)
            : base(design)
        {
            Image = null;
            Width = 0.0f;
            Height = 0.0f;
            
        }
        public ImageDesignElement(IDesign design, BitmapImage img)
            : base(design)
        {
            Image = img;
            Width = img.Width;
            Height = img.Height;
        }
        public ImageDesignElement(IDesign design, BitmapImage img, double height, double width)
            : base(design)
        {
            Image = img;
            Width = img.Width;
            Height = img.Height;
        }
        public override void Draw(DrawingContext context)
        {
            context.DrawImage(Image, new Rect(Origin.X, Origin.Y, Height, Width));            
        }
        public override XElement ToXmlElement()
        {
            var xElement = new XElement("ImageDesignElement",
                new XElement("Image", Image.UriSource),
                new XElement("Width", Width),
                new XElement("Height", Height)
                );
            AddBasePropertiesToXmlElement(xElement);
            return xElement;
        }
        public override IXmlElementConvertible FromXmlElement(XElement element)
        {
            Image = new BitmapImage(new Uri(element.Element("Image").Value));
            Width = double.Parse(element.Element("Width").Value);
            Height = double.Parse(element.Element("Height").Value);
            LoadBasePropertiesFromXmlElement(element);
            return this;
        }
    }
}
