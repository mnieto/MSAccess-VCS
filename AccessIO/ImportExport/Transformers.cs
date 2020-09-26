using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;
using dao = Microsoft.Office.Interop.Access.Dao;

namespace AccessIO {

    /// <summary>
    /// Base class for all PropertyTransform implementatios. Implements <see cref="IPropertyTransform"/>
    /// </summary>
    public abstract class PropertyTransform : IPropertyTransform {

        /// <summary>
        /// <see cref="IPropertyTransform.Transform"/> property
        /// </summary>
        public abstract string Transform(object value);

        /// <summary>
        /// Write the transformation result to a stream with help of an <see cref="ExportObject"/>
        /// </summary>
        /// <param name="export">helper class for write objects</param>
        /// <param name="property"><see cref="dao.Property"/> to transform to</param>
        public virtual void WriteTransform(ExportObject export, dao.Property property) {
            try {
                export.WriteProperty(property.Name, Transform(property));
            } catch {
                export.WriteProperty(property.Name, String.Empty);
            }
        }

        public virtual void WriteTransform(ExportObject export, Access.AccessObjectProperty property) {
            object propertyValue;
            try {
                propertyValue = property.Value;
            } catch {
                propertyValue = String.Empty;
            }
            export.WriteProperty(property.Name, Transform(propertyValue));
        }

    }

    /// <summary>
    /// Transform a property containing a full file path & name to a file name
    /// </summary>
    public class FileNameTransform : PropertyTransform {
        #region IPropertyTransform Members

        public override string Transform(object value) {
            return System.IO.Path.GetFileName((string)value);
        }

        #endregion
    }

    public class FileNameDataTypeTransform : PropertyTransform {

        public override string Transform(object value) {
            dao.Property property = (dao.Property)value; 
            return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0},{1}", property.Type , System.IO.Path.GetFileName((string)property.Value));
        }
    }

    /// <summary>
    /// Do not implement any transformation
    /// </summary>
    public class NoActionTransform : PropertyTransform {
        #region IPropertyTransform Members

        public override string Transform(object value) {
            return Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion
    }

    public class NoActionDataTypeTransoform : PropertyTransform {

        #region IPropertyTransform Members

        public override string Transform(object value) {
            dao.Property property = (dao.Property)value;
            return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0},{1}", property.Type, property.Value);
        }

        #endregion
    }

    /// <summary>
    /// Writes allways an empty string
    /// </summary>
    public class NullTransform : PropertyTransform {
        #region IPropertyTransform Members

        public override string Transform(object value) {
            return String.Empty;
        }

        public override void WriteTransform(ExportObject export, dao.Property property) {
            //write nothing at all
        }

        public override void WriteTransform(ExportObject export, Access.AccessObjectProperty accessObjectProperty) {
            //write nothing at all
        }

        #endregion
    }

}
