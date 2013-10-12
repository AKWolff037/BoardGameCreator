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
using BoardGameDesigner.Lib;
using System.Configuration;
namespace BoardGameDesigner.Styles
{
    public class MoveThumbStyle : Style        
    {
        public MoveThumbStyle()
            : base(typeof(MoveThumb))
        {
            Setters.Add(new Setter(MoveThumb.TemplateProperty, GetMoveThumbTemplate()));
        }
        public ControlTemplate GetMoveThumbTemplate()
        {
            var template = new ControlTemplate(typeof(MoveThumb));
            var rect = new Rectangle { Fill = Brushes.Transparent };
            template.RegisterName("Children", rect);
            return template;
        }
    }
}
