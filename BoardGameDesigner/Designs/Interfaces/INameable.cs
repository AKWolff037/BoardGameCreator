using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameDesigner.Designs
{
    /// <summary>
    /// Determines that a class contains a name and can be renamed
    /// </summary>
    public interface INameable
    {
        string Name { get; set; }
    }
}
