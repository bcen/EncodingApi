using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
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
                        NoiseReduction = 3,
                        Output = "flv",
                        VideoCodec = "vp6",
                        AudioCodec = "libmp3lame",
                        VideoBitrate = 0,
                        AudioBitrate = 1.5,
                        AudioSampleRate = 4000
                    },
                    new EncodingFormat()
                    {
                        Output = "mp3"
                    }
                }
            };

            string expectXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <query>
                <userid>id</userid>
                <userkey>key</userkey>
                <action>GetMediaList</action>
                <source>http://www.google.com/example1.mp3</source>
                <source>http://www.facebook.com/example2.flv</source>
                <notify>mailto://someone@gmail.com</notify>
                <instant>yes</instant>
                <format>
                    <noise_reduction>3</noise_reduction>
                    <output>flv</output>
                    <video_codec>vp6</video_codec>
                    <audio_codec>libmp3lame</audio_codec>
                    <audio_bitrate>1.5k</audio_bitrate>
                    <audio_sample_rate>4000</audio_sample_rate>
                </format>
                <format>
                    <output>mp3</output>
                </format>
            </query>";
            string actualXml = Serialize(q, "    ");

            Assert.Equal(expectXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty),
                         actualXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty));



            BasicResponseMock r = new BasicResponseMock()
            {
                Message = "Some messages",
                Errors = new List<string>()
                {
                    "error 1",
                    "error 2"
                },
                MockTest = "mock object"
            };

            expectXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <response>
                <message>Some messages</message>
                <errors>
                    <error>error 1</error>
                    <error>error 2</error>
                </errors>
                <mock>mock object</mock>
            </response>";
            actualXml = Serialize(r, "    ");

            Assert.Equal(expectXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty),
                         actualXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty));



            AddMediaResponse ar = new AddMediaResponse
            {
                MediaId = "1234",
                Message = "Added"
            };

            expectXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <response>
                <message>Added</message>
                <mediaid>1234</mediaid>
            </response>";
            actualXml = Serialize(ar, "    ");


            Assert.Equal(expectXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty),
                         actualXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty));

        }

        [Fact]
        public void TestDeserialization()
        {
            string xml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <query>
                <userid>id</userid>
                <userkey>key</userkey>
                <action>GetMediaList</action>
                <source>http://www.google.com/example1.mp3</source>
                <source>http://www.facebook.com/example2.flv</source>
                <notify>mailto://someone@gmail.com</notify>
                <instant>yes</instant>
                <format>
                    <noise_reduction>3</noise_reduction>
                    <output>flv</output>
                    <video_codec>vp6</video_codec>
                    <audio_codec>libmp3lame</audio_codec>
                    <audio_bitrate>1.5k</audio_bitrate>
                    <audio_sample_rate>4000</audio_sample_rate>
                </format>
                <format>
                    <output>mp3</output>
                </format>
            </query>";

            EncodingQuery q = Deserialize<EncodingQuery>(xml);

            Assert.NotNull(q);
            Assert.Equal("id", q.UserId);
            Assert.Equal("key", q.UserKey);
            Assert.Equal("GetMediaList", q.Action);
            Assert.Contains(new Uri("http://www.google.com/example1.mp3"), q.Sources);
            Assert.Contains(new Uri("http://www.facebook.com/example2.flv"), q.Sources);
            Assert.Equal(new Uri("mailto://someone@gmail.com"), q.Notify);
            Assert.True(q.IsInstant);

            EncodingFormat f1 = q.Formats.ElementAtOrDefault(0);
            EncodingFormat f2 = q.Formats.ElementAtOrDefault(1);

            Assert.NotNull(f1);
            Assert.Equal(3, f1.NoiseReduction);
            Assert.Equal("flv", f1.Output);
            Assert.Equal("vp6", f1.VideoCodec);
            Assert.Equal("libmp3lame", f1.AudioCodec);
            Assert.Equal(1.5D, f1.AudioBitrate);
            Assert.Equal(4000, f1.AudioSampleRate);

            Assert.NotNull(f2);
            Assert.Equal("mp3", f2.Output);


            xml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <response>
                <message>Added</message>
                <mediaid>1234</mediaid>
            </response>";

            BasicResponseMock r = Deserialize<BasicResponseMock>(xml);

            Assert.NotNull(r);
            Assert.Equal("Added", r.Message);
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
