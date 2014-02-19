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
using System.Windows.Media.Imaging;
namespace BoardGameDesigner.Designs
{
    /// <summary>
    /// Represents a Design Element, which is each individual element in a design
    /// </summary>
    public interface IDesignElement : IXmlElementConvertible, INameable, IToggleable, IRemovable
    {
        IDesign Design { get; set; }
        ICondition Condition { get; set; }
        DataColumn ValueSource { get; set; }
        DataTable DataSource { get; set; }
        int Layer { get; set; }
        void Draw(DrawingContext context);
        double X_Offset { get; set; }
        double Y_Offset { get; set; }
        Size Size { get; set; }
    }
}
