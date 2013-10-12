using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BoardGameDesigner.Designs;
using System.Configuration;
namespace BoardGameDesigner.Styles
{
    public class ThumbEllipseStyle : Style
    {
        public ThumbEllipseStyle()
            : base(typeof(Ellipse))
        {
            this.Setters.Add(new Setter(Ellipse.SnapsToDevicePixelsProperty, true));
            this.Setters.Add(new Setter(Ellipse.StrokeProperty, "#FFC8C8C8"));
            this.Setters.Add(new Setter(Ellipse.StrokeThicknessProperty, .5));
            this.Setters.Add(new Setter(Ellipse.WidthProperty, 7));
            this.Setters.Add(new Setter(Ellipse.HeightProperty, 7));
            this.Setters.Add(new Setter(Ellipse.MarginProperty, new Thickness(-2)));
            this.Setters.Add(new Setter(Ellipse.FillProperty, Brushes.Silver));

        }
    }
}
