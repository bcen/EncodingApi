using System;
using System.Xml.Serialization;
using System.Xml;
using EncodingApi.Extensions;
using System.Xml.Linq;

namespace EncodingApi
{
    /// <summary>
    /// Encapsulates AddMedia XML response.
    /// </summary>
    [XmlRoot("response")]
    public sealed class AddMediaResponse : BasicResponse
    {
        /// <summary>
        /// The ID of the added media.
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddMediaResponse()
            : base()
        {
            MediaId = String.Empty;
        }

        /// <summary>
        /// Deserializes the xml from the sepcified XElement into object instance.
        /// </summary>
        /// <param name="root">The XElement to parse from.</param>
        public override void Parse(XElement root)
        {
            if (root == null) return;

            base.Parse(root);

            // Reads <mediaid>MediaId</mediaid>
            var elem = root.Element("mediaid");
            MediaId = elem != null ? elem.Value : String.Empty;
        }

        public override void ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            Parse(root);
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            // Writes <mediaid>MediaId</mediaid>
            writer.WriteSafeElementString("mediaid", MediaId);
        }
    }
}
