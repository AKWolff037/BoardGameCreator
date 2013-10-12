using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
namespace BoardGameDesigner.Designs
{
    interface IImageDesignElement : IDesignElement
    {
        BitmapImage Image { get; set; }
        double Width { get; set; }
        double Height { get; set; }
    }
}
