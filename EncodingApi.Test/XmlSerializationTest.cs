using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using EncodingApi.Models;
using Xunit;

namespace EncodingApi.Test
{
    public class XmlSerializationTest
    {
        [Fact]
        public void TestDeserialization()
        {
            string xml =
            @"<?xml version=""1.0""?>
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
