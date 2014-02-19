using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BoardGameDesigner.IO;
using System.Data;
namespace BoardGameDesigner.Designs
{
    /// <summary>
    /// An abstract condition class that can be inherited from.  Does not directly implement the Evaluate function.
    /// </summary>
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
        /// <summary>
        /// Compares values based on the input and returns if they are equal
        /// </summary>
        /// <param name="col">The DataColumn to grab the data out of the DataRow from</param>
        /// <param name="b">The object value to compare to</param>
        /// <param name="op">The operation used in the comparison</param>
        /// <param name="type">The type of comparision to do (Text, Numeric)</param>
        /// <param name="drow">The DataRow to use </param>
        /// <returns>Returns an evaluation of whether the data is equal based on the input.</returns>
        /// <remarks>Text comparisons can only use an Operator of Equals or NotEquals, or will always return false</remarks>
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
                        return false;
                }
            }
            else if (type == ComparisonType.NumericValue)
            {
                double aNum, bNum;
                var isANum = double.TryParse(Astring, out aNum);
                var isBNum = double.TryParse(Bstring, out bNum);
                if (isANum == false || isBNum == false)
                {
                    return false;
                }
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
                        return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Parses an XElement and returns a condition typecasted to the condition specified in the Xml
        /// </summary>
        /// <param name="owner">The element that owns the condition</param>
        /// <param name="element">The XElement to parse</param>
        /// <param name="isConditionElement">Boolean flag to determine whether the element is the Condition element, or is the parent.  True if parsing the condition element itself, false if parsing its parent</param>
        /// <returns>Returns the parsed XElement as an ICondition</returns>
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
