using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using EncodingApi.Models;
using Xunit;

namespace EncodingApi.Test
{
    public class XmlSerializationTest
    {
        [Fact]
        public void TestSerialization()
        {
            EncodingQuery qry1 = new EncodingQuery();
            qry1.Action = EncodingQuery.QueryAction.ProcessMedia;
            qry1.UserId = "123";
            qry1.UserKey = "321";
            qry1.AddSourceUri("http://www.yahoo.com/test");
            qry1.Formats.Add(new EncodingFormat("mp4"));

            string expectedXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <query>
                <userid>123</userid>
                <userkey>321</userkey>
                <action>ProcessMedia</action>
                <source>http://www.yahoo.com/test</source>
                <format>
                    <output>mp4</output>
                </format>
            </query>";
            string actualXml = Serialize(qry1, "    ");

            // FIXIT: Mono ignores 'ShouldSerializeXXX' pattern.
            Assert.Equal(expectedXml.Replace(" ", String.Empty), 
                         actualXml.Replace(" ", String.Empty));
        }

        [Fact]
        public void TestDeserialization()
        {
            string xml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <query>
                <userid>7778</userid>
                <userkey>longkey</userkey>
                <action>AddMedia</action>
                <mediaid>1234</mediaid>
                <source>http://www.yahoo.com/mp4</source>
                <notify>http://callback.com/callback</notify>
                <format>
                    <output>mp4</output>
                </format>
            </query>";

            EncodingQuery qry1 = Deserialize<EncodingQuery>(xml);
            Assert.NotNull(qry1);
            Assert.Equal("7778", qry1.UserId);
            Assert.Equal("longkey", qry1.UserKey);
            Assert.Equal("AddMedia", qry1.Action);
            Assert.Equal("1234", qry1.MediaId);
            Assert.Contains(new Uri("http://www.yahoo.com/mp4"), qry1.GetAllSourceUri());
            Assert.Equal(new Uri("http://callback.com/callback"), qry1.GetNotifyUri());
            Assert.NotEmpty(qry1.Formats);
            Assert.Equal("mp4", qry1.Formats.First().Output);
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
