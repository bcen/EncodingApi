using System;
using System.Xml.Serialization;
using System.Xml;
using EncodingApi.Extensions;

namespace EncodingApi
{
    /// <summary>
    /// Encapsulates AddMedia XML response.
    /// </summary>
    [XmlRoot("response")]
    public class AddMediaResponse : BasicResponse
    {
        /// <summary>
        /// The ID of the added media.
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddMediaResponse()
        {
            MediaId = String.Empty;
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteSafeElementString("mediaid", MediaId);
        }
    }
}
