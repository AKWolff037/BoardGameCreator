using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml.Linq;
namespace BoardGameDesigner.Data
{
    public static class DataSetConverter
    {
        #region Convert To Xml
        public static XElement ConvertToXmlElement(DataSet ds)
        {
            var xElement = new XElement("DataSet",
                new XElement("Name", ds.DataSetName));
            var tables = new XElement("DataTables");
            foreach (DataTable dt in ds.Tables)
            {
                tables.Add(ConvertDataTableToXmlElement(dt));
            }
            xElement.Add(tables);
            return xElement;
        }
        public static XElement ConvertDataTableToXmlElement(DataTable dt)
        {
            var xElement = new XElement("DataTable",
                    new XElement("Name", dt.TableName));
            var colDef = new XElement("ColumnDefinitions");
            foreach (DataColumn dc in dt.Columns)
            {
                colDef.Add(ConvertDataColumnToXmlElement(dc));
            }
            var dataRows = new XElement("Rows");
            foreach (DataRow drow in dt.Rows)
            {
                dataRows.Add(ConvertDataRowToXmlElement(drow));
            }
            xElement.Add(colDef);
            xElement.Add(dataRows);
            return xElement;
        }
        public static XElement ConvertDataColumnToXmlElement(DataColumn dc)
        {
            var element = new XElement("Column", dc.ColumnName);
            element.SetAttributeValue("DataType", dc.DataType);
            element.SetAttributeValue("Ordinal", dc.Ordinal);
            return element;
        }
        public static XElement ConvertDataRowToXmlElement(DataRow drow)
        {
            var element = new XElement("Row");
            foreach (DataColumn col in drow.Table.Columns)
            {
                var data = new XElement("Column" + col.Ordinal.ToString(), drow[col]);
                element.Add(data);
            }
            return element;
        }
        #endregion

        #region Convert From Xml
        public static DataSet ConvertFromXmlElement(XElement element)
        {
            if (element == null)
                return null;
            var ds = new DataSet(element.Element("Name").Value);
            foreach (XElement tableElement in element.Element("DataTables").Elements("DataTable"))
            {
                var table = ConvertTableFromXmlElement(tableElement);
                ds.Tables.Add(table);
            }
            return ds;
        }
        public static DataTable ConvertTableFromXmlElement(XElement element)
        {
            if (element == null)
                return null;
            var dataTable = new DataTable(element.Element("Name").Value);
            foreach (XElement colElem in element.Element("ColumnDefinitions").Elements().OrderBy(el => int.Parse(el.Attribute("Ordinal").Value)))
            {
                var column = ConvertDataColumnFromXmlElement(colElem);
                dataTable.Columns.Add(column);
                column.SetOrdinal(int.Parse(colElem.Attribute("Ordinal").Value));
            }
            foreach (XElement rowElement in element.Element("Rows").Elements("Row"))
            {
                var row = ConvertDataRowFromXmlElement(rowElement, dataTable);
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
        public static DataColumn ConvertDataColumnFromXmlElement(XElement element)
        {
            if (element == null)
                return null;
            var dataColumn = new DataColumn(element.Value, Type.GetType(element.Attribute("DataType").Value));
            return dataColumn;
        }
        public static DataRow ConvertDataRowFromXmlElement(XElement element, DataTable tbl)
        {
            if (element == null)
                return null;
            var dataRow = tbl.NewRow();
            foreach (DataColumn col in tbl.Columns)
            {
                var data = element.Element("Column" + col.Ordinal.ToString()).Value;                
                dataRow[col] = data;
            }
            return dataRow;
        }
        #endregion
    }
}
