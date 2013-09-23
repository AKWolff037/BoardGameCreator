using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
namespace BoardGameDesigner.Designs
{
    public interface ITextDesignElement : IDesignElement
    {
        string Text { get; set; }
        double FontSize { get; set; }
        FontFamily Font { get; set; }
        FontWeight Weight { get; set; }
        Brush Color { get; set; }
        FontStyle Style { get; set; }
    }
}
