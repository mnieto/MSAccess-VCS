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
            try {
                App.Application.LoadFromText((Access.AcObjectType)ObjectType, Name, fileName);
            } catch (System.Runtime.InteropServices.COMException ex) {
                if (ex.ErrorCode == -2146826003) {   
                    //Error 2285, cannot create the file xxxx.
                    //This error occurs in Access 2003 when Access file format is Access 2002-2003. 
                    //Format must be changed prior to load from file into the database
                    //see http://stackoverflow.com/questions/208397/loadfromtext-gives-error-2285-microsoft-office-access-cant-create-the-output-f
                    //and http://support.microsoft.com/kb/927680
                    throw new Exception(Properties.ImportRes.InvalidFileFormat, ex);
                } else
                    throw;
            }

        } 
    }
}
