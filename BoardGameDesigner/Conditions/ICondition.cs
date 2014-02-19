using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGameDesigner.IO;
using System.Data;
namespace BoardGameDesigner.Designs
{
    /// <summary>
    /// Represents a condition that can be applied to a design element on a dataset
    /// </summary>
    public interface ICondition : IXmlElementConvertible
    {
        /// <summary>
        /// The element that contains the condition
        /// </summary>
        IDesignElement OwnerElement { get; }
        /// <summary>
        /// Evaluates the condition against the data row and returns whether it is true or false
        /// </summary>
        /// <param name="drow">The DataRow to compare the condition against.</param>
        /// <returns>Returns the evaluation of the condition (true/false)</returns>
        bool Evaluate(DataRow drow);
    }
}
