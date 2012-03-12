using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EncodingApi.Models
{
    public abstract class XmlModelBase
    {
        protected XElement Root { get; private set; }

        public XmlModelBase()
            : this("<root/>")
        {
        }

        public XmlModelBase(string xml)
        {
            Root = XElement.Parse(xml);
        }

        /// <summary>
        /// Returns an XML representation of the object.
        /// </summary>
        /// <returns>An one lined XML string.</returns>
        public override string ToString()
        {
            return ToString(Root, false);
        }

        /// <summary>
        /// Returns an XML representation of the object.
        /// </summary>
        /// <param name="isPrettify">To test if the output should be correctly indented.</param>
        /// <returns>An XML string.</returns>
        public string ToString(bool isPrettify)
        {
            return ToString(Root, isPrettify);
        }

        protected string ToString(XContainer xContainer, bool isPrettify)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = isPrettify;
            settings.IndentChars = "  ";
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                xContainer.WriteTo(writer);
            }

            // Hacks. Replaces the encoding="UTF-16" to "UTF-8"
            if (sb.Length >= 37)
            {
                sb.Replace(Encoding.Unicode.WebName, Encoding.UTF8.WebName, 0, 37);
            }
            return sb.ToString();
        }

        protected string GetXmlElementInnerText(string elementName)
        {
            if (String.IsNullOrEmpty(elementName))
                throw new ArgumentException("elementName cannot be null or empty");

            XElement element = Root.Element(elementName);
            return element == null ? String.Empty : element.Value;
        }

        protected void SetXmlElementInnerText(string elementName, string text)
        {
            if (String.IsNullOrEmpty(elementName))
                throw new ArgumentException("elementName cannot be null or empty");

            // Search for the node with the name 'elementName'.
            XElement element = Root.Element(elementName);
            
            if (element == null && !String.IsNullOrEmpty(text))
            {
                element = new XElement(elementName, text);
                Root.Add(element);
            }
            else if (element != null && String.IsNullOrEmpty(text))
            {
                element.Remove();
            }
            else
            {
                element.Value = text;
            }
        }
    }
}
