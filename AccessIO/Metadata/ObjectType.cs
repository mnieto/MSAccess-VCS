using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Access;

namespace AccessIO {

    /// <summary>
    /// maps, as possible, Access object types <see cref="Microsoft.Office.Interop.Access.AcObjectType">AcObjectType</see> and adds custom object types
    /// </summary>
    public enum ObjectType {
        General = AcObjectType.acDefault,

        /// <summary>
        /// Dao tables
        /// </summary>
        Table = AcObjectType.acTable,

        /// <summary>
        /// Dao queries
        /// </summary>
        Query = AcObjectType.acQuery,

        /// <summary>
        /// Forms
        /// </summary>
        Form = AcObjectType.acForm,

        /// <summary>
        /// Reports
        /// </summary>
        Report = AcObjectType.acReport,

        /// <summary>
        /// Macro objecs
        /// </summary>
        /// <remarks>dao.Container's name for Macros is 'Scripts'</remarks>
        Macro = AcObjectType.acMacro,

        /// <summary>
        /// Modules and Classes
        /// </summary>
        Module = AcObjectType.acModule,

        /// <summary>
        /// Data Access Page
        /// </summary>
        /// <remarks>Partially supported because SaveAsText, export to binary format and this object is deprecatted begining with Office 2007</remarks>
        DataAccessPage = AcObjectType.acDataAccessPage,

        ServerView = AcObjectType.acServerView,
        
        /// <summary>
        /// SQL Database diagram (for adp projects)
        /// </summary>
        Diagram = AcObjectType.acDiagram,
        
        /// <summary>
        /// SQL Server stored procedures (for adp projects)
        /// </summary>
        StoredProcedure = AcObjectType.acStoredProcedure,

        /// <summary>
        /// SQL Server functions (for adp projects, in Queries container)
        /// </summary>
        Function = AcObjectType.acFunction,

        /// <summary>
        /// Auxiliar object for serialization of Table objects
        /// </summary>
        Property = 100,

        /// <summary>
        /// Database references
        /// </summary>
        References = 101,

        /// <summary>
        /// Auxiliar object for serialization of Table objects
        /// </summary>
        Index = 102,

        /// <summary>
        /// Auxiliar object for serialization of indexes of a table
        /// </summary>
        IndexField = 103,

        /// <summary>
        /// Auxiliar object for serialization of Table objects
        /// </summary>
        Field = 104,

        /// <summary>
        /// Relations of dao.Tables
        /// </summary>
        Relations = 105,

        /// <summary>
        /// Auxiliar object for serialization of Relations
        /// </summary>
        FieldRelation = 106,

        /// <summary>
        /// Auxiliar enum value for serialization of mdb databse properties
        /// </summary>
        DatabaseDao = 107,

        /// <summary>
        /// Auxiliar enum value for serialization of adp database properties
        /// </summary>
        DatabasePrj = 108
    }
}
