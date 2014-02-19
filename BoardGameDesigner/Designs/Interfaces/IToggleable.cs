using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameDesigner.Designs
{
    /// <summary>
    /// IToggleable classes implement a boolean to determine if they are enabled or not
    /// </summary>
    public interface IToggleable
    {
        bool Enabled { get; set; }
    }
}
