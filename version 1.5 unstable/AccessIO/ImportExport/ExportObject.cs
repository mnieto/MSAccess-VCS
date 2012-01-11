using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using dao;

namespace AccessIO {

    /// <summary>
    /// Helper class to write to text files Access custom objects
    /// </summary>
    public class ExportObject {

        private StreamWriter sw;

        /// <summary>
        /// Tab size for indentation. Tabs are filled with spaces (Default 4)
        /// </summary>
        public static int TabSize { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sw">Stream where write to</param>
        public ExportObject(StreamWriter sw) {
            TabSize = 4;
            this.sw = sw;
        }

        /// <summary>
        /// Current identation level. Initial value is 0
        /// </summary>
        /// <remarks>
        /// Each WriteBegin increment the identation level and each WriteEnd decrement it
        /// </remarks>
        public int Indent { get; set; }

        /// <summary>
        /// Writes a complete custom object, surrounding it with Begin and End
        /// </summary>
        /// <param name="customObject">object to write to the stream</param>
        /// <remarks>
        /// Internally, it calls the <see cref="ICustomObject.SaveProperties"/> method
        /// </remarks>
        public void WriteObject(AuxiliarObject customObject) {
            WriteBegin(customObject.ClassName);
            customObject.SaveProperties(this);
            WriteEnd();        
        }

        /// <summary>
        /// Writes a complete custom object, surrounding it with Begin and End
        /// </summary>
        /// <param name="customObject">object to write to the stream</param>
        /// <param name="name">name of the object that is wrote in the Begin clause</param>
        public void WriteObject(AuxiliarObject customObject, string name) {
            WriteBegin(customObject.ClassName, name);
            customObject.SaveProperties(this);
            WriteEnd();
        }

        /// <summary>
        /// Write a Begin clause and increment the identation level
        /// </summary>
        /// <param name="className">name of the class for the Begin clause</param>
        public void WriteBegin(string className) {
            sw.WriteLine(String.Format("{0}{1} {2}", new String(' ', Indent * TabSize), Properties.Resources.Begin, className));
            Indent++;
        }

        /// <summary>
        /// Write a Begin clause and increment the identation level
        /// </summary>
        /// <param name="className">name of the class for the Begin clause</param>
        /// <param name="name">object's name to write in the Begin clause</param>
        public void WriteBegin(string className, string name) {
            sw.WriteLine(String.Format("{0}{1} {2} {3}", new String(' ', Indent * TabSize), Properties.Resources.Begin, className, name));
            Indent++;
        }

        /// <summary>
        /// Write a property name, without value
        /// </summary>
        /// <param name="propertyName"></param>
        public void WriteProperty(string propertyName) {
            sw.WriteLine(String.Format("{0}{1}:", new String(' ', Indent * TabSize), propertyName));
        }

        /// <summary>
        /// Write a property name and its value (if any)
        /// </summary>
        /// <param name="propertyName">name of the property</param>
        /// <param name="value">value of the property. Value is writen with InvariantCulture cultural reference</param>
        public void WriteProperty(string propertyName, object value) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}:", new String(' ', Indent * TabSize), propertyName);
            if (value != null)
                sb.AppendFormat(" {0}", Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
            sw.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Write a property name and its value
        /// </summary>
        /// <param name="propertyName">name of the property</param>
        /// <param name="value"><see cref="dao.Property"/> property</param>
        public void WriteProperty(string propertyName, dao.Property value) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}:", new String(' ', Indent * TabSize), propertyName);
            if (value != null && value.Value != null)
                sb.AppendFormat(" {0}", Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
            sw.WriteLine(sb.ToString());

        }

        /// <summary>
        /// Write a list of properties. The specific properties to write are proposed by the <see cref="ICustomObject"/> itself
        /// </summary>
        /// <param name="accessObj">object whose properties must write</param>
        public void WriteProperties(ICustomObject accessObj) {
            WriteProperties(accessObj, accessObj.Properties);
        }

        /// <summary>
        /// Write a list of properties.
        /// </summary>
        /// <param name="accessObj">object whose properties must write</param>
        /// <param name="properties">list of property names</param>
        public void WriteProperties(ICustomObject accessObj, List<string> properties) {
            foreach (string item in properties) {
                WriteProperty(item, accessObj[item]);
            }
        }
        
        /// <summary>
        /// Writes a End clause and decrement de indentation level
        /// </summary>
        public void WriteEnd() {
            Indent--;
            sw.WriteLine(String.Format("{0}{1}", new String(' ', Indent * TabSize), Properties.Resources.End));
        }

        [System.Diagnostics.Conditional("DEBUG")]
        internal void ListProperties(string objectName, dao.Properties properties) {
            System.Diagnostics.Debug.WriteLine(objectName);
            for (int i = 1; i < properties.Count; i++) {
                dao.Property property = properties[i];
                object value = null;
                try {
                    value = property.Value;
                } catch {
                    value = "#Error";
                }
                System.Diagnostics.Debug.WriteLine(String.Format("\t{0}:\t{1}", property.Name, value));
            }
        }
    }
}
