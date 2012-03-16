using System;
using System.Xml.Serialization;

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
        [XmlElement("MediaID")]
        public string MediaId { get; set; }

        /// <summary>
        /// To test whether to serialize MediaId or not.
        /// </summary>
        /// <returns>True if MediaId is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeMediaId() { return !String.IsNullOrEmpty(MediaId); }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddMediaResponse()
        {
            MediaId = String.Empty;
        }
    }
}
