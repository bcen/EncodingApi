using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EncodingApi
{
    /// <summary>
    /// Represents object that is XML serializable/deserializable.
    /// <remarks>
    /// This abstract class implements the IXmlSerializable interface and has
    /// one addition method: ReadXml(XElement).
    /// Subclass should use ReadXml(XElement) instead of ReadXml(XmlReader), since 
    /// XElement provides easier access with Linq to XML than the forward-only XML reader.
    /// By default, ReadXml(XmlReader) method will create a XElement from the reader
    /// and pass it to ReadXml(XElement).
    /// </remarks>
    /// </summary>
    public abstract class XmlSerializableObject : IXmlSerializable
    {
        public virtual XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads the XML representation into this object instance.
        /// </summary>
        /// <param name='reader'>The XmlReader to read from.</param>
        public virtual void ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            ReadXml(root);
        }

        /// <summary>
        /// Reads the XML representation into this object instance.
        /// </summary>
        /// <remarks>
        /// Implementer of this method must ensure that all base class properties are correctly
        /// deserialized from its XML, or the derived class should call <c>base.Read(XElement)</c>
        /// to have the properties read from the XElement.
        /// </remarks>
        /// <param name='reader'>The XElement to read from.</param>
        public abstract void ReadXml(XElement root);

        /// <summary>
        /// Writes this object into XML representation.
        /// </summary>
        /// <remarks>
        /// When overriden in derived class, call base.WriteXml(XmlWriter) to ensure that
        /// all properties are correctly written to the writer.
        /// </remarks>
        /// <param name='writer'>The XmlWriter to write to.</param>
        public abstract void WriteXml(XmlWriter writer);
    }
}
