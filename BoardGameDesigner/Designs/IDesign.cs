using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using BoardGameDesigner.IO;
namespace BoardGameDesigner.Designs
{
    public interface IDesign : IXmlElementConvertible, INameable, IToggleable
    {
        IDesignManager DesignManager { get; set; }
        List<IDesignElement> DesignElements { get; }
        BitmapImage Template { get; set; }
        void Draw();
        BitmapImage DrawImage();
    }
}
