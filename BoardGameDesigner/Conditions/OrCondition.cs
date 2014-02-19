using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
namespace BoardGameDesigner.Designs
{
    /// <summary>
    /// A condition that may contain any number of sub-conditions.  Will evaluate true if any subconditions evaluate as true.
    /// </summary>
    public class OrCondition : Condition
    {
        public List<ICondition> Conditions { get; private set; }
        public OrCondition(IDesignElement owner)
            : base(owner)
        {
            Conditions = new List<ICondition>();
        }
        public OrCondition(IDesignElement owner, params ICondition[] conditions)
            : base (owner)
        {
            Conditions = new List<ICondition>(conditions);
        }
        public override bool Evaluate(DataRow drow)
        {
            foreach (ICondition cond in Conditions)
            {
                if (cond.Evaluate(drow) == true)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Convert the condition into an XML element in order to save it
        /// </summary>
        /// <returns>Returns the XElement value of the condition</returns>
        public override XElement ToXmlElement()
        {
            var condElement = new XElement("OrCondition");
            var conditions = new XElement("Conditions");
            foreach (ICondition cond in Conditions)
            {
                var condElem = cond.ToXmlElement();
                conditions.Add(condElem);
            }
            condElement.Add(conditions);
            return condElement;
        }
        /// <summary>
        /// Converts a Condition from an XElement
        /// </summary>
        /// <param name="element">The XElement to convert the condition from</param>
        /// <returns>Returns the condition as an IXmlElementConvertible</returns>
        public override IO.IXmlElementConvertible FromXmlElement(XElement element)
        {
            Conditions.Clear();
            foreach (XElement condElem in element.Element("Conditions").Elements())
            {
                var cond = Condition.ParseElement(OwnerElement, condElem, true);
                Conditions.Add(cond);
            }
            return this;
        }

    }
}
