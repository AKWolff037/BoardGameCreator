using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace BoardGameDesigner.IO
{
    public interface IXmlElementConvertible
    {
        XElement ToXmlElement();
        IXmlElementConvertible FromXmlElement(XElement element);
    }
}
