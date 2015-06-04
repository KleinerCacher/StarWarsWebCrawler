using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace WebCrawler
{
    public static class XmlGenerator
    {
        public static void GenerateXml(string filename, AllIndexData data)
        {
            XmlSerializer serializer = new XmlSerializer(data.GetType());
            TextWriter WriteFileStream = new StreamWriter(filename);
            serializer.Serialize(WriteFileStream, data);
        }
    }
}
