using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using BoardGameDesigner.IO;
namespace BoardGameDesigner.Designs
{
    public enum LayerMoveType
    {
        UP,
        DOWN
    }
    public interface IDesign : IXmlElementConvertible, INameable, IToggleable, IRemovable
    {
        IDesignManager DesignManager { get; set; }
        List<IDesignElement> DesignElements { get; }
        BitmapImage Template { get; set; }
        //void Draw();
        RenderTargetBitmap DrawImage();
        void ReorderDesignElements(IDesignElement elementToMove, LayerMoveType direction);
    }
}
