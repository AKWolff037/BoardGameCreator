using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
namespace BoardGameDesigner.Designs
{
    /// <summary>
    /// A simple condition that checks two fields to see if they are equal
    /// </summary>
    public class SimpleCondition : Condition
    {
        /// <summary>
        /// The comparision DataColumn to pull data from the DataRow from when calling the Evaluate method
        /// </summary>
        public DataColumn ComparisonColumn { get; set; }
        /// <summary>
        /// The value to compare the column to
        /// </summary>
        public object CompareValue { get; set; }
        /// <summary>
        /// The operator to use in the comparison
        /// </summary>
        public ConditionalOperator Operator { get; set; }
        /// <summary>
        /// The type of comparision to do (Text, Numeric)
        /// </summary>
        public ComparisonType CompareType { get; set; }
        private Type CompareValueType { get; set; }
        public SimpleCondition(IDesignElement owner)
            : base(owner)
        {
            ComparisonColumn = null;
            CompareValue = null;
            Operator = ConditionalOperator.Equals;
            CompareType = ComparisonType.None;
            CompareValueType = null;
        }
        public SimpleCondition(IDesignElement owner, DataColumn col, ConditionalOperator op, object compareVal, ComparisonType type = ComparisonType.None)
            : base(owner)
        {
            ComparisonColumn = col;
            Operator = op;
            CompareValue = compareVal;
            CompareValueType = compareVal.GetType();
            if (type != ComparisonType.None)
            {
                CompareType = type;
            }
            else
            {
                if (CompareValueType == typeof(string) || CompareValueType == typeof(char))
                {
                    CompareType = ComparisonType.TextValue;
                }
                else if (CompareValueType == typeof(int) || CompareValueType == typeof(double) || CompareValueType == typeof(decimal) || CompareValueType == typeof(float) || CompareValueType == typeof(long))
                {
                    CompareType = ComparisonType.NumericValue;
                }
            }
        }
        public override bool Evaluate(DataRow drow)
        {
            return CompareValues(ComparisonColumn, CompareValue, Operator, CompareType, drow);
        }
        /// <summary>
        /// Convert the condition into an XML element in order to save it
        /// </summary>
        /// <returns>Returns the XElement value of the condition</returns>
        public override XElement ToXmlElement()
        {
            var condElement = new XElement("SimpleCondition");
            if (ComparisonColumn != null)
            {
                var colElement = Data.DataSetConverter.ConvertDataColumnToXmlElement(ComparisonColumn);
                condElement.Add(colElement);
            }
            if (CompareValue != null)
            {
                var compareValElement = new XElement("CompareValue", CompareValue);
                compareValElement.SetAttributeValue("Type", CompareValueType.ToString());
                condElement.Add(compareValElement);
            }
            var operatorElement = new XElement("Operator", Operator);
            condElement.Add(operatorElement);
            var typeElement = new XElement("Type", CompareType);
            condElement.Add(typeElement);
            
            return condElement;
        }
        /// <summary>
        /// Converts a Condition from an XElement
        /// </summary>
        /// <param name="element">The XElement to convert the condition from</param>
        /// <returns>Returns the condition as an IXmlElementConvertible</returns>
        public override IO.IXmlElementConvertible FromXmlElement(XElement element)
        {
            var ownerTable = OwnerElement.DataSource;
            if (ownerTable != null)
            {
                var compareColumn = Data.DataSetConverter.ConvertDataColumnFromXmlElement(element.Element("Column"));
                ComparisonColumn = ownerTable.Columns[compareColumn.ColumnName];
            }
            CompareValue = element.Element("CompareValue").Value;
            CompareValueType = System.Type.GetType(element.Element("CompareValue").Attribute("Type").Value);
            Operator = (ConditionalOperator)Enum.Parse(typeof(ConditionalOperator), element.Element("Operator").Value);
            CompareType = (ComparisonType)Enum.Parse(typeof(ComparisonType), element.Element("Type").Value);            
            return this;
        }
    }
}
