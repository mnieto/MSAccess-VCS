using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;

namespace AccessIO {
    /// <summary>
    /// Objets that have an implmentation for LoadFromText and SaveAsText
    /// </summary>
    public class StandardObject : AccessObject {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app"><see cref="AccessApp"/> object</param>
        /// <param name="name">Access object's name</param>
        /// <param name="objectType"><see cref="ObjectType"/> specifing the concrete type of Access object</param>
        public StandardObject(AccessApp app, string name, ObjectType objectType): base(app, name, objectType) {
        }

        public override void Save(string fileName) {
            MakePath(System.IO.Path.GetDirectoryName(fileName));
            App.Application.SaveAsText((Access.AcObjectType)ObjectType, Name, fileName);
        }

        public override void Load(string fileName) {
            App.Application.LoadFromText((Access.AcObjectType)ObjectType, Name, fileName);
        } 
    }
}
