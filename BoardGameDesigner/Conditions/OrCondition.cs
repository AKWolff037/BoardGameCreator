using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
namespace BoardGameDesigner.Designs
{
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
