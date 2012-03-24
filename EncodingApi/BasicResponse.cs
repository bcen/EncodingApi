using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
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
        /// List of errors.
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

        /// <summary>
        /// Deserializes the xml from the sepcified XElement into object instance.
        /// </summary>
        /// <param name="root">The XElement to parse from.</param>
        public virtual void Parse(XElement root)
        {
            if (root == null) return;

            var elem = root.Element("message");
            Message = elem != null ? elem.Value : String.Empty;

            elem = root.Element("errors");
            if (elem != null)
            {
                var childs = elem.Elements("error");
                if (childs != null)
                {
                    foreach (var item in childs)
                    {
                        Errors.Add(item.Value);
                    }
                }
            }
        }

        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
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
