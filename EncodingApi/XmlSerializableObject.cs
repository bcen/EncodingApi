using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EncodingApi
{
    /// <summary>
    /// Represents object that is serializable to, and deserializable from XML.
    /// <remarks>
    /// This abstract class implements the IXmlSerializable interface and provides
    /// one addition method: ReadXml(XElement). The default implementation of
    /// ReadXml(XmlReader) is create a XElement from the XML reader and pass it off to
    /// ReadXml(XElement).
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
        /// <param name='reader'>The XmlReader to read from.</param>
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
