using System;
namespace AccessIO {
    public interface IObjectName {
        string Name { get; set; }
        ObjectType ObjectType { get; set; }
        //string Path { get; }
    }
}
