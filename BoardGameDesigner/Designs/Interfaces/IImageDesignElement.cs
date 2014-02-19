using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
namespace BoardGameDesigner.Designs
{
    /// <summary>
    /// Represents an image-based design element
    /// </summary>
    public interface IImageDesignElement : IDesignElement
    {
        BitmapImage Image { get; set; }
    }
}
