using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using BoardGameDesigner.IO;
namespace BoardGameDesigner.Designs
{
    public class TextDesignElement : DesignElement, ITextDesignElement
    {
        public string Text { get; set; }
        public double FontSize { get; set; }
        public FontWeight Weight { get; set; }
        public FontFamily Font { get; set; }       
        public Brush Color { get; set; }
        public FontStyle Style { get; set; }
        public TextDesignElement(IDesign design)
            : base(design)
        {
            Text = string.Empty;
            FontSize = 12.0;
            Weight = new FontWeight();
            Font = new System.Windows.Media.FontFamily("Times New Roman");
            Color = Brushes.Black;
            Style = FontStyles.Normal;
            Name = "New Text Design Element";
        }

        public override void Draw(DrawingContext context)
        {
            var formattedText = new FormattedText(Text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Font, Style, Weight, new FontStretch()), FontSize, Color);
            //Fit text to maximum size it can go depending on the size of the design element
            while (formattedText.Height < Size.Height && formattedText.Width < Size.Width)
            {
                FontSize++;
                formattedText = new FormattedText(Text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Font, Style, Weight, new FontStretch()), FontSize, Color);
            }
            while (formattedText.Height > Size.Height || formattedText.Width > Size.Width)
            {
                FontSize--;
                if (FontSize < 1)
                    break;
                formattedText = new FormattedText(Text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Font, Style, Weight, new FontStretch()), FontSize, Color);
            }
            context.DrawText(formattedText, new Point(X_Offset, Y_Offset));
        }
        public override XElement ToXmlElement()
        {
            var xElement = new XElement("TextDesignElement",
                new XElement("Text", Text),
                new XElement("FontSize", FontSize),
                new XElement("Weight", Weight),
                new XElement("Font", ProjectIOManager.ConvertFontToXmlElement(Font)), //TODO: Implement Font
                new XElement("Color", Color),
                new XElement("Style", Style)
            );
            AddBasePropertiesToXmlElement(xElement);
            return xElement;
        }
        public override IXmlElementConvertible FromXmlElement(XElement element)
        {
            this.Text = element.Element("Text").Value;
            this.FontSize = double.Parse(element.Element("FontSize").Value);
            //this.Weight = double.Parse(element.Element("PenWidth").Value);
            //TODO: Parse Font and weight and everything
            this.Font = ProjectIOManager.ConvertFontFromXmlElement(element.Element("Font"));
            //TODO: Parse color
            //if (element.Element("Color-ScA") != null)
            //{
            //    var sca = float.Parse(element.Element("Color-ScA").Value);
            //    var scb = float.Parse(element.Element("Color-ScB").Value);
            //    var scg = float.Parse(element.Element("Color-ScG").Value);
            //    var scr = float.Parse(element.Element("Color-ScR").Value);
            //    this.Color = Color.FromScRgb(sca, scr, scg, scb);
            //}
            LoadBasePropertiesFromXmlElement(element);
            return this;
        }
    }
}
