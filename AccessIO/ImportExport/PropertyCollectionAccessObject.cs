using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSAccess = Microsoft.Office.Interop.Access;

namespace AccessIO {

    /// <summary>
    /// AccessObjectProperties management
    /// </summary>
    class PropertyCollectionAccessObject {

        /// <summary>
        /// Properties of a AccessObject like CurrentProject
        /// </summary>
        public MSAccess.AccessObjectProperties Properties { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="properties">Properties collection to manage</param>
        public PropertyCollectionAccessObject(MSAccess.AccessObjectProperties properties) {
            Properties = properties;
        }

        /// <summary>
        /// Assign a property value. If the property do not exists in the property collection, add the property to the collection
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="propertyValue">Value of the property</param>
        public void AddProperty(string propertyName, object propertyValue) {
            try {
                if (propertyValue != null)
                    Properties[propertyName].Value = propertyValue;
            } catch (System.Runtime.InteropServices.COMException ex) {
                if (ex.ErrorCode == -2146825833) {
                    Properties.Add(propertyName, propertyValue);
                } else
                    throw;
            }
        
        }
    }
}
