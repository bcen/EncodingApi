using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using Xunit;

namespace EncodingApi.Test
{
    public class XmlSerializationTest
    {
        [Fact]
        public void TestSerialization()
        {
            EncodingQuery q = new EncodingQuery()
            {
                UserId = "id",
                UserKey = "key",
                Action = "GetMediaList",
                MediaId = "1234",
                Notify = new Uri("mailto://someone@gmail.com"),
                IsInstant = true,
                Sources = new List<Uri>()
                {
                    new Uri("http://www.google.com/example1.mp3"),
                    new Uri("http://www.facebook.com/example2.flv")
                },
                Formats = new List<EncodingFormat>()
                {
                    new EncodingFormat()
                    {
                        Output = "flv",
                        NoiseReduction = 3,
                        AudioSampleRate = 0
                    }
                }
            };

            string expectXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <query>
                <userid>id</userid>
                <userkey>key</userkey>
                <action>GetMediaList</action>
                <mediaid>1234</mediaid>
                <source>http://www.google.com/example1.mp3</source>
                <source>http://www.facebook.com/example2.flv</source>
                <notify>mailto://someone@gmail.com</notify>
                <instant>yes</instant>
                <format>
                    <noise_reduction>3</noise_reduction>
                    <output>flv</output>
                </format>
            </query>";
            string actualXml = Serialize(q, "    ");

            Assert.Equal(expectXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty),
                         actualXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty));
        }

        [Fact]
        public void TestDeserialization()
        {

        }

        public virtual string Serialize<T>(T obj, string indentChars) where T : class, new()
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            StringBuilder sb = new StringBuilder();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            XmlWriterSettings settings = new XmlWriterSettings();

            if (!String.IsNullOrEmpty(indentChars))
            {
                settings.IndentChars = indentChars;
                settings.Indent = true;
            }
            ns.Add("", "");

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                ser.Serialize(writer, obj, ns);
            }

            if (sb.Length >= 37)
            {
                sb.Replace(Encoding.Unicode.WebName, Encoding.UTF8.WebName, 0, 37);
            }

            return sb.ToString();
        }

        public T Deserialize<T>(string xml) where T : class, new()
        {
            T obj = default(T);
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(xml))
            {
                obj = (T)ser.Deserialize(reader);
            }

            return obj;
        }
    }
}
