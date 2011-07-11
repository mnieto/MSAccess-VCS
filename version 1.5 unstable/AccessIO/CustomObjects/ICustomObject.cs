using System;
namespace AccessIO {
    /// <summary>
    /// Objets that do not have an implmentation for LoadFromText and SaveAsText
    /// </summary>
    public interface ICustomObject: IObjecOptions {

        /// <summary>
        /// Gets or sets the underlying Access object
        /// </summary>
        object DaoObject { get; set; }

        /// <summary>
        /// Name of the specific object type
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Options for this object
        /// </summary>
        OptionsObj Options { get; }
        
        /// <summary>
        /// List of the property names to save
        /// </summary>
        System.Collections.Generic.List<string> Properties { get; }

        /// <summary>
        /// Gets the value of the specified property
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns></returns>
        object this[string propertyName] { get; }

    }
}
