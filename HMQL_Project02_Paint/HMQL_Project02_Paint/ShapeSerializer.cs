using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using static HMQL_Project02_Paint.MainWindow;
using IContract;
using System.Xml;
using System.Xml.Serialization;

namespace HMQL_Project02_Paint
{
    static class SerializeInterface
    {
        public static void SerializeShapes(List<IShapeEntity> drawnShapes, string filePath)
        {
            // Create a list of IShape
            var shapes = new ListOfIShape(drawnShapes);

            // Create the XML serializer
            var xmlSerializer = new XmlSerializer(typeof(ListOfIShape));

            // Create the XML writer
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            using (var xmlWriter = XmlWriter.Create(fileStream, new XmlWriterSettings { Indent = true }))
            {
                // Serialize the shapes to XML
                xmlSerializer.Serialize(xmlWriter, shapes);
            }
        }

        public static List<IShapeEntity> DeserializeShapes(string filePath)
        {
            // Create the XML serializer
            var xmlSerializer = new XmlSerializer(typeof(ListOfIShape));

            // Create the XML reader
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            using (var xmlReader = XmlReader.Create(fileStream))
            {
                // Deserialize the XML into a list of IShape objects
                var shapes = (ListOfIShape)xmlSerializer.Deserialize(xmlReader);
                return shapes.ToList();
            }
        }
    }

    public class ListOfIShape : List<IShapeEntity>, IXmlSerializable
    {
        public ListOfIShape() : base() { }

        public ListOfIShape(List<IShapeEntity> shapes) : base(shapes) { }

        #region IXmlSerializable
        public System.Xml.Schema.XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement) return;
            reader.ReadStartElement("ListOfIShape");
            while (reader.IsStartElement("IShape"))
            {
                Type type = Type.GetType(reader.GetAttribute("AssemblyQualifiedName"));
                XmlSerializer serial = new XmlSerializer(type);

                reader.ReadStartElement("IShape");
                this.Add((IShapeEntity)serial.Deserialize(reader));
                reader.ReadEndElement(); //IShape
            }
            reader.ReadEndElement(); //ListOfIShape
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (IShapeEntity shape in this)
            {
                writer.WriteStartElement("IShape");
                writer.WriteAttributeString("AssemblyQualifiedName", shape.GetType().AssemblyQualifiedName);
                XmlSerializer xmlSerializer = new XmlSerializer(shape.GetType());
                xmlSerializer.Serialize(writer, shape);
                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
