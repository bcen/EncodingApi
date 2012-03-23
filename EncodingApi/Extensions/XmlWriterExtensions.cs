using System;
using System.Xml;

namespace EncodingApi.Extensions
{
    public static class XmlWriterExtensions
    {
        public static void WriteSafeElementString(this XmlWriter writer, string localName,
                                                  string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                writer.WriteElementString(localName, value.Trim());
            }
        }
    }
}
