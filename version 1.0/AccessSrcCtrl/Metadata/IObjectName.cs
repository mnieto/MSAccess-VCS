using System;
namespace AccessIO {
    public interface IObjectName {

        /// <summary>
        /// Object's name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Object's type
        /// </summary>
        ObjectType ObjectType { get; set; }

    }
}
