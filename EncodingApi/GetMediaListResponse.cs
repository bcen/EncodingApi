using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
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

        /// <summary>
        /// Reads XML from <c>root</c> into this object instance.
        /// </summary>
        /// <param name="root">The XElement to read from.</param>
        protected override void ReadXml(XElement root)
        {
            if (root == null) return;

            base.ReadXml(root);

            // Reads <media>...</media>
            var elems = root.Elements("media");
            if (elems != null)
            {
                if (MediaList == null)
                {
                    MediaList = new List<Media>();
                }
                foreach (var item in elems)
                {
                    Media m = new Media();
                    if (m is IXmlSerializable)
                    {
                        using (XmlReader r = item.CreateReader())
                        {
                            r.MoveToContent();
                            ((IXmlSerializable)m).ReadXml(r);
                            MediaList.Add(m);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads the XML representation into this object instance.
        /// </summary>
        /// <param name='reader'>The XmlReader to read from.</param>
        public override void ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            ReadXml(root);
        }

        /// <summary>
        /// Writes this object into XML representation.
        /// </summary>
        /// <param name='writer'>The XmlWriter to write to.</param>
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

            /// <summary>
            /// Reads the XML representation into this object instance.
            /// </summary>
            /// <param name='reader'>The XmlReader to read from.</param>
            void IXmlSerializable.ReadXml(XmlReader reader)
            {
                XElement root = XElement.ReadFrom(reader) as XElement;
                if (root == null) return;

                // Reads <mediafile></mediafile>
                var elem = root.Element("mediafile");
                MediaFile = elem != null ? new Uri(elem.Value) : null;

                // Reads <mediaid></mediaid>
                elem = root.Element("mediaid");
                MediaId = elem != null ? elem.Value : String.Empty;

                // Reads <mediastatus></mediastatus>
                elem = root.Element("mediastatus");
                MediaStatus = elem != null ? elem.Value : String.Empty;

                
                DateTime d = DateTime.MinValue;
                
                // Reads <createdate></createdate>
                elem = root.Element("createdate");
                if (elem != null)
                {
                    if (DateTime.TryParse(elem.Value, out d))
                    {
                        CreateDate = d;
                    }
                }

                // Reads <startdate></startdate>
                elem = root.Element("startdate");
                if (elem != null)
                {
                    if (DateTime.TryParse(elem.Value, out d))
                    {
                        StartDate = d;
                    }
                }

                // Reads <finishdate></finishdate>
                elem = root.Element("finishdate");
                if (elem != null)
                {
                    if (DateTime.TryParse(elem.Value, out d))
                    {
                        FinishDate = d;
                    }
                }
            }

            /// <summary>
            /// Writes this object into XML representation.
            /// </summary>
            /// <param name='writer'>The XmlWriter to write to.</param>
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
