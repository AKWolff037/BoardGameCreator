using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGameDesigner.Designs;
using BoardGameDesigner.IO;
namespace BoardGameDesigner.Projects
{
    public interface IProject : IXmlElementConvertible, INameable
    {
        IDesignManager DesignManager { get; set; }
        string ProjectFilePath { get; set; }
        bool IsDirty { get; set; }
        void Save();
        event System.Windows.RoutedEventHandler Saved;
    }
}
