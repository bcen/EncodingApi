using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using EncodingApi.Extensions;

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
        /// Builds this object instance from <c>root</c>.
        /// </summary>
        /// <param name="root">The XElement to build from.</param>
        protected override void Build(XElement root)
        {
            if (root == null) return;

            base.Build(root);

            // Reads <mediaid></mediaid>
            var elem = root.Element("mediaid");
            MediaId = elem != null ? elem.Value : String.Empty;
        }

        /// <summary>
        /// Reads the XML representation into this object instance.
        /// </summary>
        /// <param name='reader'>The XmlReader to read from.</param>
        public override void ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            Build(root);
        }

        /// <summary>
        /// Writes this object into XML representation.
        /// </summary>
        /// <param name='writer'>The XmlWriter to write to.</param>
        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            // Writes <mediaid>MediaId</mediaid>
            writer.WriteSafeElementString("mediaid", MediaId);
        }
    }
}
