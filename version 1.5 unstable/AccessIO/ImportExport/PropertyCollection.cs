using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessIO {

    /// <summary>
    /// Dao properties management
    /// </summary>
    class PropertyCollection {

        /// <summary>
        /// Properties of a dao object like table, field or database
        /// </summary>
        public dao.Properties Properties { get; private set; }

        /// <summary>
        /// dao object it self
        /// </summary>
        public object DaoObject { get; private set; }

        /// <summary>
        /// Ctor. Create a PropertyCollection object to manage the properties of a dao object
        /// </summary>
        /// <param name="daoObject">dao object like a field, table or database</param>
        /// <param name="properties">The Properties property of <paramref name="daoObject"/> object</param>
        public PropertyCollection(object daoObject, dao.Properties properties) {
            if (daoObject == null)
                throw new ArgumentNullException("daoObject");
            if (properties == null)
                throw new ArgumentNullException("properties");
            DaoObject = daoObject;
            Properties = properties;
        }

        /// <summary>
        /// Adds a MS-Access property to the dao object
        /// </summary>
        /// <param name="props">List of properties to read to or write to a file</param>
        /// <param name="propertyName">property name to add to the dao object</param>
        /// <param name="dataType">data type of the property</param>
        /// <remarks>
        /// Adding a property to a dao object is trickly: if the property is already added we assign de value
        /// else, we create the property and add to the properties collection
        /// </remarks>
        public void AddOptionalProperty(Dictionary<string, object> props, string propertyName, dao.DataTypeEnum dataType) {
            if (props.ContainsKey(propertyName) && props[propertyName] != null) {
                try {
                    Properties[propertyName].Value = props[propertyName];
                } catch (System.Runtime.InteropServices.COMException ex) {
                    if (ex.ErrorCode == -2146825018) {   //Property don't exists in the properties collection
                        object[] parameters = new object[] { propertyName, dataType, props[propertyName] };
                        //All the dao objects implements a interface with CreateProperty method. But all of them are diferent interfaces: there isn't a common interface
                        //Calling InvokeMember we can do a generic code to call CreateProperty of al the objects
                        Properties.Append((dao.Property)(DaoObject.GetType().InvokeMember("CreateProperty", System.Reflection.BindingFlags.InvokeMethod, null, DaoObject, parameters)));
                    } else
                        throw;
                }
            }
        }

        public void TryWriteProperty(ExportObject export, string propertyName) {
            if (PropertyHasValue(propertyName))
                export.WriteProperty(propertyName, Properties[propertyName].Value);
            else
                export.WriteProperty(propertyName);
        }

        public bool HasProperty(string propertyName) {
            try {
                dao.Property property = Properties[propertyName];
                return true;
            } catch {
                return false;
            }
        }

        public bool PropertyHasValue(string propertyName) {
            return (HasProperty(propertyName) && PropertyHasValue(Properties[propertyName]));
        }

        public bool PropertyHasValue(dao.Property property) {
            try {
                object dummy = property.Value;
                return true;
            } catch {
                return false;
            }
        }

    }
}
