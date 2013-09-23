using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Media;
using System.Xml.Linq;
using BoardGameDesigner.IO;
using System.Windows;
namespace BoardGameDesigner.Designs
{
    public interface IDesignElement : IXmlElementConvertible, INameable, IToggleable
    {
        IDesign Design { get; set; }
        ICondition Condition { get; set; }
        DataColumn ValueSource { get; set; }
        DataTable DataSource { get; set; }
        int Layer { get; set; }
        void Draw(DrawingContext context);
        Rect Origin { get; set; }
    }
}
