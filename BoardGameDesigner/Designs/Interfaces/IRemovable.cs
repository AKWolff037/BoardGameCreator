using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameDesigner.Designs
{
    /// <summary>
    /// Specifies that a class implements the Remove method to delete it or remove it from a parent object
    /// </summary>
    public interface IRemovable
    {
        void Remove();
    }
}
