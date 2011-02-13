using System;
namespace AccessIO {
    /// <summary>
    /// Objets that do not have an implmentation for LoadFromText and SaveAsText
    /// </summary>
    public interface ICustomObject: IObjectName {
        /// <summary>
        /// Gets or sets the underlying Access object
        /// </summary>
        object DaoObject { get; set; }
        /// <summary>
        /// Name of the specific object type
        /// </summary>
        string ClassName { get; }
        /// <summary>
        /// List of the property names to save
        /// </summary>
        System.Collections.Generic.List<string> Properties { get; }
        object this[string propertyName] { get; }
        void SaveProperties(ExportObject export);
    }
}
