using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameDesigner.Designs
{
    public interface IToggleable
    {
        bool Enabled { get; set; }
    }
}
