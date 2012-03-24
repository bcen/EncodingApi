using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using EncodingApi.Extensions;

namespace EncodingApi
{
    [XmlRoot("query")]
    public class EncodingQuery : IXmlSerializable
    {
        /// <summary>
        /// A unique user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User's unique authentication key string.
        /// </summary>
        public string UserKey { get; set; }

        /// <summary>
        /// The action to be performed in the API request.
        /// </summary>
        public string Action { get; set; }
        
        /// <summary>
        /// A unique identifier for the media.
        /// This field must be specified for the following actions: UpdateMedia, CancelMedia, 
        /// GetStatus.
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// Source media file. Must be specified only for AddMedia and AddMediaBenchmark actions.
        /// </summary>
        public ICollection<Uri> Sources { get; set; }

        /// <summary>
        /// Can be either an HTTP(S) URL for the script with which the result will be posted,
        /// or a mailto: link with email address for which the result info will be sent. 
        /// This field may be specified for AddMedia and AddMediaBenchmark actions.
        /// </summary>
        public Uri Notify { get; set; }

        /// <summary>
        /// Set to 'yes' to initiate the encoding process immediately when source video begins
        /// downloading to our processing center as opposed to waiting until after the
        /// download has completed. Also, this feature can be used when source media is
        /// still uploading to the specified source FTP location - our system will recognize if
        /// the source file size increases while downloading, or soon after, and the "tail" will
        /// be downloaded and concatenated.
        /// </summary>
        public bool IsInstant { get; set; }

        /// <summary>
        /// One or more format elements are required for AddMedia and UpdateMedia actions.
        /// </summary>
        public ICollection<EncodingFormat> Formats { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EncodingQuery()
        {
            UserId = String.Empty;
            UserKey = String.Empty;
            Action = String.Empty;
            MediaId = String.Empty;
            Sources = new List<Uri>();
            Notify = null;
            IsInstant = false;
            Formats = new List<EncodingFormat>();
        }

        public static EncodingQuery CreateGetMediaListQuery()
        {
            return new EncodingQuery()
            {
                Action = EncodingQuery.QueryAction.GetMediaList,
            };
        }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Deserializes the xml from <c>reader</c>.
        /// </summary>
        /// <param name="reader">The XmlReader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            if (root == null) return;

            var elem = root.Element("userid");
            UserId = elem != null ? elem.Value : String.Empty;

            elem = root.Element("userkey");
            UserKey = elem != null ? elem.Value : String.Empty;

            elem = root.Element("action");
            Action = elem != null ? elem.Value : String.Empty;

            elem = root.Element("mediaid");
            MediaId = elem != null ? elem.Value : String.Empty;

            var elemList = root.Elements("source");
            if (elemList != null)
            {
                if (Sources == null)
                {
                    Sources = new List<Uri>();
                }

                foreach (var item in elemList)
                {
                    Sources.Add(new Uri(item.Value));
                }
            }

            elem = root.Element("notify");
            Notify = elem == null ? null : new Uri(elem.Value);

            elem = root.Element("instant");
            string text = elem != null ? elem.Value : "no";
            IsInstant = text.StartsWith("yes");

            var s = root.Elements("format");
            if (s != null)
            {
                if (Formats == null)
                {
                    Formats = new List<EncodingFormat>();
                }

                foreach (var item in s)
                {
                    using (XmlReader r = item.CreateReader())
                    {
                        EncodingFormat f = new EncodingFormat();
                        if (f is IXmlSerializable)
                        {
                            r.MoveToContent();
                            ((IXmlSerializable)f).ReadXml(r);
                            Formats.Add(f);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Serializes the object to xml.
        /// </summary>
        /// <param name="writer">The XmlWriter.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteSafeElementString("userid", UserId);
            writer.WriteSafeElementString("userkey", UserKey);
            writer.WriteSafeElementString("action", Action);
            writer.WriteSafeElementString("mediaid", MediaId);

            if (Sources != null)
            {
                foreach (Uri uri in Sources)
                {
                    writer.WriteSafeElementString("source", uri.AbsoluteUri);
                }
            }

            if (Notify != null)
            {
                writer.WriteSafeElementString("notify", Notify.AbsoluteUri);
            }

            if (IsInstant)
            {
                writer.WriteSafeElementString("instant", "yes");
            }

            if (Formats != null)
            {
                foreach (EncodingFormat f in Formats)
                {
                    if (f is IXmlSerializable)
                    {
                        writer.WriteStartElement("format");
                        ((IXmlSerializable)f).WriteXml(writer);
                        writer.WriteEndElement();
                    }
                }
            }
        }

        /// <summary>
        /// A convenience class to look up availiable query actions.
        /// </summary>
        public static class QueryAction
        {
            public readonly static string AddMedia = "AddMedia";
            public readonly static string AddMediaBenchmark = "AddMediaBenchmark";
            public readonly static string UpdateMedia = "UpdateMedia";
            public readonly static string ProcessMedia = "ProcessMedia";
            public readonly static string CancelMedia = "CancelMedia";
            public readonly static string GetMediaList = "GetMediaList";
            public readonly static string GetStatus = "GetStatus";
            public readonly static string GetMediaInfo = "GetMediaInfo";
            public readonly static string RestartMedia = "RestartMedia";
            public readonly static string RestartMediaErrors = "RestartMediaErrors";
            public readonly static string RestartMediaTask = "RestartMediaTask";
        }
    }
}
