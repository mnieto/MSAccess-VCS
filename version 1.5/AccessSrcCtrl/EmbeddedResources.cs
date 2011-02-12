using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AccessIO {

    class EmbeddedResources {
        public static List<string> GetObjectAttributes(string className) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string streamName = String.Concat(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, ".ObjectAttributes.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(assembly.GetManifestResourceStream(streamName));
            XmlNodeList nodes = doc.SelectNodes(String.Format(@"//AccessClasses/Class[@Name='{0}']/Property", className));
            List<string> lst = new List<string>(nodes.Count);
            foreach (XmlNode node in nodes)
                lst.Add(node.FirstChild.Value);
            return lst;

        }
    }
}
