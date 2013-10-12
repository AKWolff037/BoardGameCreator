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
using BoardGameDesigner.Lib;
namespace BoardGameDesigner.Styles
{
    public class DesignElementStyle : Style
    {
        public DesignElementStyle()
            : base(typeof(ContentControl))
        {
            //this.Setters.Add(new Setter(ContentControl.TemplateProperty, CreateDesignTemplate()));
        }

        //public Control CreateResizeControl()
        //{
        //    var ctrl = new Control { Name = "resizer" };
        //    ctrl.Style = new ResizeControlStyle();

        //}

        //public ControlTemplate CreateDesignTemplate()
        //{

        //}
        public Grid CreateMoveThumbGrid()
        {
            var moveThumbGrid = new Grid
            {
                DataContext = new Binding { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) },
                Children =
                {
                    new MoveThumb { Style = new MoveThumbStyle(), Cursor=Cursors.SizeAll }
                }
            };
            return moveThumbGrid;
        }
        public Grid CreateResizeHitboxGrid()
        {
            var resizeHitGrid = new Grid
            {
                IsHitTestVisible = false,
                Opacity = 1,
                Margin = new Thickness(-3),
                Children =
                {
                    new Rectangle { SnapsToDevicePixels=true, StrokeThickness=1, Margin= new Thickness(1), Stroke=Brushes.Black, StrokeDashArray=new DoubleCollection(){4, 4}},
                    new Ellipse { HorizontalAlignment=HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, Style=new ThumbEllipseStyle() },
                    new Ellipse { HorizontalAlignment=HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Style=new ThumbEllipseStyle() },
                    new Ellipse { HorizontalAlignment=HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, Style=new ThumbEllipseStyle() },
                    new Ellipse { HorizontalAlignment=HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Style=new ThumbEllipseStyle() }
                }
            };

            return resizeHitGrid;
        }
        public Grid CreateResizeGrid()
        {
            var resizeGrid = new Grid
            {
                Opacity = 0,
                Margin = new Thickness(-3),
                Children =
                {
                    new ResizeThumb { Height = 3, Cursor = Cursors.SizeNS, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Stretch },
                    new ResizeThumb { Height = 3, Cursor = Cursors.SizeWE, VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Left },
                    new ResizeThumb { Height = 3, Cursor = Cursors.SizeWE, VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Right },
                    new ResizeThumb { Height = 3, Cursor = Cursors.SizeNS, VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Stretch },
                    new ResizeThumb { Height = 7, Cursor = Cursors.SizeNWSE, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left },
                    new ResizeThumb { Height = 7, Cursor = Cursors.SizeNESW, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right },
                    new ResizeThumb { Height = 7, Cursor = Cursors.SizeNESW, VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Left },
                    new ResizeThumb { Height = 7, Cursor = Cursors.SizeNWSE, VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Right }
                }
            };
            return resizeGrid;
        }
    }
}
