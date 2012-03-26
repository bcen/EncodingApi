using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EncodingApi
{
    /// <summary>
    /// Encapsulates RestartMediaErrors XML response.
    /// </summary>
    [XmlRoot("response")]
    public sealed class RestartMediaErrorsResponse : BasicResponse
    {
        /// <summary>
        /// Reads XML from <c>root</c> into this object instance.
        /// </summary>
        /// <param name="root">The XElement to read from.</param>
        public override void ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            base.ReadXml(root);
        }
    }
}
