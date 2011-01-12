using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Access;

namespace AccessIO {

    /// <summary>
    /// maps, as possible, Access object types <see cref="Microsoft.Office.Interop.Access.AcObjectType">AcObjectType</see> and adds custom object types
    /// </summary>
    public enum ObjectType {
        Default = AcObjectType.acDefault,
        Table = AcObjectType.acTable,
        Query = AcObjectType.acQuery,
        Form = AcObjectType.acForm,
        Report = AcObjectType.acReport,
        Macro = AcObjectType.acMacro,
        Module = AcObjectType.acModule,
        DataAccessPage = AcObjectType.acDataAccessPage,
        ServerView = AcObjectType.acServerView,
        Diagram = AcObjectType.acDiagram,
        StoredProcedure = AcObjectType.acStoredProcedure,
        Function = AcObjectType.acFunction,
        Property = 100,
        References = 101,
        Index = 102,
        IndexField = 103,
        Field = 104,
        Relations = 105,
        FieldRelation = 106,
        DatabaseDao = 107,
        DatabasePrj = 108
    }
}
