using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml.Linq;
using BoardGameDesigner.IO;
using BoardGameDesigner.Projects;
namespace BoardGameDesigner.Designs
{
    /// <summary>
    /// Represents a Design Manager, which contains a collection of Designs as well as a DataSet
    /// </summary>
    public interface IDesignManager : IXmlElementConvertible
    {
        IProject Project { get; set; }
        List<IDesign> Designs { get; }
        DataSet Dataset { get; set; }        
    }
}
