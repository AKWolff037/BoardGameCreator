using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
namespace BoardGameDesigner.Designs
{
    public class SimpleCondition : Condition
    {
        public DataColumn ComparisonColumn { get; set; }
        public object CompareValue { get; set; }
        public ConditionalOperator Operator { get; set; }
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
        public override IO.IXmlElementConvertible FromXmlElement(XElement element)
        {
            var ownerTable = OwnerElement.DataSource;
            if (ownerTable != null)
            {
                var compareColumn = Data.DataSetConverter.ConvertDataColumnFromXmlElement(element.Element("Column"));
                ComparisonColumn = ownerTable.Columns[compareColumn.ColumnName];
                CompareValue = element.Element("CompareValue").Value;
                CompareValueType = System.Type.GetType(element.Element("CompareValue").Attribute("Type").Value);
                Operator = (ConditionalOperator)Enum.Parse(typeof(ConditionalOperator), element.Element("Operator").Value);
                CompareType = (ComparisonType)Enum.Parse(typeof(ComparisonType), element.Element("Type").Value);
            }
            return this;
        }
    }
}
