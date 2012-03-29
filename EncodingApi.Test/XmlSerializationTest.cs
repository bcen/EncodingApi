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
        public void ShouldThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                EncodingFormat f = new EncodingFormat();
                f.NoiseReduction = 7;
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                EncodingFormat f = new EncodingFormat();
                f.CropBottom = 3;
            });
        }

        [Fact]
        public void TestSerialization()
        {
            // EncodingQuery
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
                        Output = "mp3",
                        AudioVolume = 84,
                        VideoFrameSize = new VideoDimension(840, 640),
                        FadeIn = new FadingEffectTimeParameters(0D, 3.5D),
                        CropLeft = 4,
                        CropRight = 6,
                        CropTop = 8,
                        CropBottom = 10,
                        KeepAspectRatio = false
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
                    <audio_volume>84</audio_volume>
                    <size>840x640</size>
                    <fade_in>0:3.5</fade_in>
                    <crop_left>4</crop_left>
                    <crop_right>6</crop_right>
                    <crop_top>8</crop_top>
                    <crop_bottom>10</crop_bottom>
                    <keep_aspect_ratio>no</keep_aspect_ratio>
                </format>
            </query>";
            string actualXml = Serialize(q, "    ");

            Assert.Equal(expectXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty),
                         actualXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty));



            // AddMediaResponse
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


            
            // GetMediaListResponse
            DateTime d;
            GetMediaListResponse gr = new GetMediaListResponse
            {
                Message = "Added some messages",
                MediaList = new List<GetMediaListResponse.Media>()
                {
                    new GetMediaListResponse.Media()
                    {
                        MediaFile = new Uri("http://www.example.com/example.mp4"),
                        MediaId = "8945307",
                        MediaStatus = "Error",
                        CreateDate = DateTime.TryParse("2012-03-13 14:53:51", out d) ? d : DateTime.MinValue,
                        StartDate = DateTime.TryParse("2012-03-13 14:54:28", out d) ? d : DateTime.MinValue,
                        FinishDate = DateTime.TryParse("0000-00-00 00:00:00", out d) ? d : DateTime.MinValue
                    }
                }
            };
            expectXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <response>
                <message>Added some messages</message>
                <media>
                    <mediafile>http://www.example.com/example.mp4</mediafile>
                    <mediaid>8945307</mediaid>
                    <mediastatus>Error</mediastatus>
                    <createdate>2012-03-13 14:53:51</createdate>
                    <startdate>2012-03-13 14:54:28</startdate>
                    <finishdate>0000-00-00 00:00:00</finishdate>
                </media>
            </response>";
            actualXml = Serialize(gr, "    ");


            Assert.Equal(expectXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty),
                         actualXml.Replace(" ", String.Empty).Replace(Environment.NewLine, String.Empty));
        }

        [Fact]
        public void TestDeserialization()
        {
            // EncodingQuery
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
                    <audio_volume>84</audio_volume>
                    <size>840x640</size>
                    <fade_in>0:3.5</fade_in>
                    <crop_left>4</crop_left>
                    <crop_right>6</crop_right>
                    <crop_top>8</crop_top>
                    <crop_bottom>10</crop_bottom>
                    <keep_aspect_ratio>no</keep_aspect_ratio>
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
            Assert.Equal(84, f2.AudioVolume);
            Assert.Equal(840, f2.VideoFrameSize.Width);
            Assert.Equal(640, f2.VideoFrameSize.Height);
            Assert.Equal(0D, f2.FadeIn.StartTime);
            Assert.Equal(3.5D, f2.FadeIn.Duration);
            Assert.Equal(4, f2.CropLeft);
            Assert.Equal(6, f2.CropRight);
            Assert.Equal(8, f2.CropTop);
            Assert.Equal(10, f2.CropBottom);
            Assert.False(f2.KeepAspectRatio);

            // AddMediaResponse
            xml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <response>
                <message>Added</message>
                <mediaid>1234</mediaid>
                <errors>
                    <error>some error</error>
                </errors>
            </response>";

            AddMediaResponse ar = Deserialize<AddMediaResponse>(xml);

            Assert.NotNull(ar);
            Assert.Equal("1234", ar.MediaId);
            Assert.Equal("Added", ar.Message);
            Assert.Contains("some error", ar.Errors);

            // GetMediaListResponse
            xml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <response>
                <media>
                    <mediafile>http://www.example.com/example.mp4</mediafile>
                    <mediaid>8945307</mediaid>
                    <mediastatus>Error</mediastatus>
                    <createdate>2012-03-13 14:53:51</createdate>
                    <startdate>2012-03-13 14:54:28</startdate>
                    <finishdate>0000-00-00 00:00:00</finishdate>
                </media>
            </response>";

            GetMediaListResponse gr = Deserialize<GetMediaListResponse>(xml);

            Assert.NotNull(ar);
            Assert.Equal(new Uri("http://www.example.com/example.mp4"), gr.MediaList.First().MediaFile);
            Assert.Equal("8945307", gr.MediaList.First().MediaId);

            DateTime d = DateTime.MinValue;;
            DateTime.TryParse("2012-03-13 14:53:51", out d);
            Assert.Equal(d, gr.MediaList.First().CreateDate);

            DateTime.TryParse("2012-03-13 14:54:28", out d);
            Assert.Equal(d, gr.MediaList.First().StartDate);

            DateTime.TryParse("0000-00-00 00:00:00", out d);
            Assert.Equal(d, gr.MediaList.First().FinishDate);
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
