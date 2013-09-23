using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGameDesigner.IO;
using System.Data;
namespace BoardGameDesigner.Designs
{
    public interface ICondition : IXmlElementConvertible
    {
        IDesignElement OwnerElement { get; }
        bool Evaluate(DataRow drow);
    }
}
