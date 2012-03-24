using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using EncodingApi.Extensions;

namespace EncodingApi
{
    /// <summary>
    /// Encapsulates GetMediaList XML response.
    /// </summary>
    [XmlRoot("response")]
    public sealed class GetMediaListResponse : BasicResponse
    {
        /// <summary>
        /// List of media meta from server.
        /// </summary>
        public ICollection<Media> MediaList { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GetMediaListResponse()
        {
            MediaList = new List<Media>();
        }

        public override void Parse(XElement root)
        {
            if (root == null) return;

            base.Parse(root);


        }

        public override void ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            Parse(root);
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            // Writes <media>...</media>
            if (MediaList != null)
            {
                foreach (var m in MediaList)
                {
                    if (m is IXmlSerializable)
                    {
                        writer.WriteStartElement("media");
                        ((IXmlSerializable)m).WriteXml(writer);
                        writer.WriteEndElement();
                    }
                }
            }
        }

        /// <summary>
        /// Media meta class.
        /// </summary>
        public sealed class Media : IXmlSerializable
        {
            /// <summary>
            /// The source URL of the media.
            /// </summary>
            public Uri MediaFile { get; set; }

            /// <summary>
            /// The ID of the media.
            /// </summary>
            public string MediaId { get; set; }
            
            /// <summary>
            /// The status of the media.
            /// </summary>
            public string MediaStatus { get; set; }

            /// <summary>
            /// The date when the media is first created in the queue.
            /// </summary>
            public DateTime CreateDate { get; set; }

            /// <summary>
            /// The date when the media is first started in the processing queue.
            /// </summary>
            public DateTime StartDate { get; set; }

            /// <summary>
            /// The date when the media is finished.
            /// </summary>
            public DateTime FinishDate { get; set; }

            /// <summary>
            /// Default constructor.
            /// </summary>
            public Media()
            {
                MediaFile = null;
                MediaId = String.Empty;
                MediaStatus = String.Empty;
                CreateDate = DateTime.MinValue;
                StartDate = DateTime.MinValue;
                FinishDate = DateTime.MinValue;
            }

            System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
            {
                return null;
            }

            public void Parse(XElement root)
            {
            }

            void IXmlSerializable.ReadXml(XmlReader reader)
            {
            }

            void IXmlSerializable.WriteXml(XmlWriter writer)
            {
                // Writes <mediafile></mediafile>
                if (MediaFile != null)
                {
                    writer.WriteSafeElementString("mediafile", MediaFile.AbsoluteUri);
                }

                // Writes <mediaid></mediaid>
                writer.WriteSafeElementString("mediaid", MediaId);

                // Writes <mediastatus></mediastatus>
                writer.WriteSafeElementString("mediastatus", MediaStatus);

                // Writes <createdate></createdate>
                writer.WriteSafeElementString("createdate", 
                                              !CreateDate.Equals(DateTime.MinValue)
                                              ? CreateDate.ToString("yyyy-MM-dd HH:mm:ss", 
                                                CultureInfo.GetCultureInfo("en-US"))
                                              : "0000-00-00 00:00:00");

                // Writes <startdate></startdate>
                writer.WriteSafeElementString("startdate",
                                              !StartDate.Equals(DateTime.MinValue)
                                              ? StartDate.ToString("yyyy-MM-dd HH:mm:ss", 
                                                CultureInfo.GetCultureInfo("en-US"))
                                              : "0000-00-00 00:00:00");

                // Writes <finishdate></finishdate>
                writer.WriteSafeElementString("finishdate",
                                              !FinishDate.Equals(DateTime.MinValue)
                                              ? FinishDate.ToString("yyyy-MM-dd HH:mm:ss", 
                                                CultureInfo.GetCultureInfo("en-US"))
                                              : "0000-00-00 00:00:00");
            }
        }
    }
}
