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
        /// Reads the XML representation into this object instance.
        /// </summary>
        /// <param name='reader'>The XmlReader to read from.</param>
        public override void ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            base.Build(root);
        }
    }
}
