using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EncodingApi
{
    [XmlRoot("response")]
    public class RestartMediaErrorsResponse : BasicResponse
    {
        public override void ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            base.Build(root);
        }
    }
}
