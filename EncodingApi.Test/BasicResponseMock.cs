using System;
using System.Xml.Serialization;
using System.Xml;
using EncodingApi.Extensions;

namespace EncodingApi.Test
{
    [XmlRoot("response")]
    public class BasicResponseMock : BasicResponse
    {
        public string MockTest { get; set; }

        public BasicResponseMock ()
        {
            MockTest = String.Empty;
        }

        public override void ReadXml (XmlReader reader)
        {
            base.ReadXml(reader);
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteSafeElementString("mock", MockTest);
        }
    }
}

