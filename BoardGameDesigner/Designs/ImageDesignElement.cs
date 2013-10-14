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
        public ImageDesignElement(IDesign design)
            : base(design)
        {
            Image = null;
            Name = "New Image Design Element";
        }
        public ImageDesignElement(IDesign design, BitmapImage img)
            : base(design)
        {
            Image = img;
            Name = "New Image Design Element";
            Size = new Size(Image.Width, Image.Height);
        }
        public ImageDesignElement(IDesign design, BitmapImage img, double height, double width)
            : base(design)
        {
            Image = img;
            Name = "New Image Design Element";
            Size = new Size(width, height);
        }

        public override void Draw(DrawingContext context)
        {
            context.DrawImage(Image, new Rect(X_Offset, Y_Offset, Size.Width, Size.Height));            
        }
        public override XElement ToXmlElement()
        {
            var xElement = new XElement("ImageDesignElement",
                new XElement("Image", Image.UriSource)
                );
            AddBasePropertiesToXmlElement(xElement);
            return xElement;
        }
        public override IXmlElementConvertible FromXmlElement(XElement element)
        {
            Image = new BitmapImage(new Uri(element.Element("Image").Value));
            LoadBasePropertiesFromXmlElement(element);
            return this;
        }
    }
}
