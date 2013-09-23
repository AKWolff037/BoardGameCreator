using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BoardGameDesigner.IO;
using System.Data;
namespace BoardGameDesigner.Designs
{
    public abstract class Condition : ICondition
    {
        public IDesignElement OwnerElement { get; private set; }
        public Condition(IDesignElement owner)
        {
            OwnerElement = owner;
        }
        public enum ConditionalOperator
        {
            Equals,
            GreaterThan,
            GreaterThanOrEquals,
            LessThan,
            LessThanOrEquals,
            NotEquals
        }
        public enum ComparisonType
        {
            TextValue,
            NumericValue,
            None
        }
        public abstract bool Evaluate(DataRow drow);
        public abstract XElement ToXmlElement();
        public abstract IXmlElementConvertible FromXmlElement(XElement element);
        public bool CompareValues(DataColumn col, object b, ConditionalOperator op, ComparisonType type, DataRow drow)
        {
            var Astring = drow[col].ToString();
            var Bstring = b.ToString();
            if (type == ComparisonType.TextValue)
            {
                switch (op)
                {
                    case ConditionalOperator.Equals:
                        return Astring.Equals(Bstring);
                    case ConditionalOperator.NotEquals:
                        return Astring != Bstring;
                    default:
                        throw new InvalidOperationException("Cannot do a text comparision with operator of " + op);
                }
            }
            else if (type == ComparisonType.NumericValue)
            {
                var aNum = double.Parse(Astring);
                var bNum = double.Parse(Bstring);
                switch (op)
                {
                    case ConditionalOperator.Equals:
                        return aNum == bNum;
                    case ConditionalOperator.GreaterThan:
                        return aNum > bNum;
                    case ConditionalOperator.GreaterThanOrEquals:
                        return aNum >= bNum;
                    case ConditionalOperator.LessThan:
                        return aNum < bNum;
                    case ConditionalOperator.LessThanOrEquals:
                        return aNum <= bNum;
                    case ConditionalOperator.NotEquals:
                        return aNum != bNum;
                    default:
                        throw new InvalidOperationException("Cannot do a numeric comparision with operator of " + op);
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot do a comparison without a comparison type and operator.");
            }
        }
        public static ICondition ParseElement(IDesignElement owner, XElement element, bool isConditionElement = false)
        {
            if (!isConditionElement)
            {
                var simple = element.Element("SimpleCondition");
                var andCond = element.Element("AndCondition");
                var orCond = element.Element("OrCondition");
                if (simple != null)
                    return new SimpleCondition(owner).FromXmlElement(simple) as SimpleCondition;
                if (andCond != null)
                    return new AndCondition(owner).FromXmlElement(andCond) as AndCondition;
                if (orCond != null)
                    return new OrCondition(owner).FromXmlElement(orCond) as OrCondition;
                if (element.Elements().Count(e => e.Name.LocalName.Contains("Condition")) > 0)
                {
                    throw new InvalidOperationException("Invalid condition found in node: " + element.Name.LocalName + " is not a recognized condition");
                }
            }
            else
            {
                var isSimple = element.Name.LocalName == "SimpleCondition";
                var isAnd = element.Name.LocalName == "AndCondition";
                var isOr = element.Name.LocalName == "OrCondition";
                if (isSimple)
                    return new SimpleCondition(owner).FromXmlElement(element) as SimpleCondition;
                if (isAnd)
                    return new AndCondition(owner).FromXmlElement(element) as AndCondition;
                if (isOr)
                    return new OrCondition(owner).FromXmlElement(element) as OrCondition;
                if (element.Name.LocalName.Contains("Condition"))
                    throw new InvalidOperationException("Invalid condition node: " + element.Name.LocalName + " is not a recognized condition");
            }
            return null;            
        }
    }
}
