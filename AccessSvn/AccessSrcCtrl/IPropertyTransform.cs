using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {

    /// <summary>
    /// Interface for transform properties in other values
    /// </summary>
    public interface IPropertyTransform {
        /// <summary>
        /// Transoform a property to another related value
        /// </summary>
        /// <param name="value">Original value of the property</param>
        string Transform(object value);

        /// <summary>
        /// Writes the result of a transformation with the <see cref="ExportObject"/>
        /// </summary>
        /// <param name="export">ExportObject used to write properties</param>
        /// <param name="value">Value to transform and, then, write</param>
        void WriteTransform(ExportObject export, dao.Property value);
    }
}
