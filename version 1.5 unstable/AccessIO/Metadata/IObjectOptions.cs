using System;
namespace AccessIO {
    public interface IObjecOptions {

        /// <summary>
        /// Object's name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Object's type
        /// </summary>
        ObjectType ObjectType { get; set; }

        /// <summary>
        /// Options for this object
        /// </summary>
        OptionsObj Options { get; }

    }
}
