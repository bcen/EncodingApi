using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using EncodingApi.Extensions;

namespace EncodingApi
{
    /// <summary>
    /// Base class for all response class.
    /// </summary>
    [XmlRoot("response")]
    public abstract class BasicResponse : IXmlSerializable
    {
        /// <summary>
        /// Message from response.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// List of error.
        /// </summary>
        public ICollection<string> Errors { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BasicResponse()
        {
            Message = string.Empty;
            Errors = new List<string>();
        }

        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            if (root == null) return;

            var elem = root.Element("message");
            Message = elem != null ? elem.Value : String.Empty;
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteSafeElementString("message", Message);

            if (Errors != null)
            {
                int count = Errors.Count;
                if (count > 0)
                {
                    writer.WriteStartElement("errors");
                    foreach (var item in Errors)
                    {
                        writer.WriteSafeElementString("error", item);
                    }
                    writer.WriteEndElement();
                }
            }
        }
    }
}
