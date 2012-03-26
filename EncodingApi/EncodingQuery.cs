using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using EncodingApi.Extensions;

namespace EncodingApi
{
    /// <summary>
    /// Encapsulates the encoding query.
    /// </summary>
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

        /// <summary>
        /// Creates a GetMediaList query action.
        /// </summary>
        /// <returns>The query for GetMediaList.</returns>
        public static EncodingQuery CreateGetMediaListQuery()
        {
            return new EncodingQuery()
            {
                Action = EncodingQuery.QueryAction.GetMediaList
            };
        }

        /// <summary>
        /// Creates a RestartMediaErrors with the specified media.
        /// </summary>
        /// <param name="mediaId">The ID of the media to be restarted.</param>
        /// <returns>The query for RestartMediaErrors.</returns>
        public static EncodingQuery CreateRestartMediaErrorsQuery(string mediaId)
        {
            return new EncodingQuery()
            {
                Action = EncodingQuery.QueryAction.RestartMediaErrors,
                MediaId = mediaId
            };
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

            // Reads <userid></userid>
            var elem = root.Element("userid");
            UserId = elem != null ? elem.Value : String.Empty;

            // Reads <userkey></userkey>
            elem = root.Element("userkey");
            UserKey = elem != null ? elem.Value : String.Empty;

            // Reads <action></action>
            elem = root.Element("action");
            Action = elem != null ? elem.Value : String.Empty;

            // Reads <mediaid></mediaid>
            elem = root.Element("mediaid");
            MediaId = elem != null ? elem.Value : String.Empty;

            // Reads <source></source>
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

            // Reads <notify></notify>
            elem = root.Element("notify");
            Notify = elem == null ? null : new Uri(elem.Value);

            // Reads <instant></instant>
            elem = root.Element("instant");
            string text = elem != null ? elem.Value : "no";
            IsInstant = text.StartsWith("yes");

            // Reads <format>...</format>
            var s = root.Elements("format");
            if (s != null)
            {
                if (Formats == null)
                {
                    Formats = new List<EncodingFormat>();
                }
                foreach (var item in s)
                {
                    EncodingFormat f = new EncodingFormat();
                    if (f is IXmlSerializable)
                    {
                        using (XmlReader r = item.CreateReader())
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
        /// Writes this object into XML representation.
        /// </summary>
        /// <param name='writer'>The XmlWriter to write to.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            // Writes <userid></userid>
            writer.WriteSafeElementString("userid", UserId);

            // Writes <userkey></userkey>
            writer.WriteSafeElementString("userkey", UserKey);

            // Writes <action></action>
            writer.WriteSafeElementString("action", Action);

            // Writes <mediaid></mediaid>
            writer.WriteSafeElementString("mediaid", MediaId);

            // Writes <source></source>
            if (Sources != null)
            {
                foreach (Uri uri in Sources)
                {
                    writer.WriteSafeElementString("source", uri.AbsoluteUri);
                }
            }

            // Writes <notify></notify>
            if (Notify != null)
            {
                writer.WriteSafeElementString("notify", Notify.AbsoluteUri);
            }

            // Writes <instant></instant>
            if (IsInstant)
            {
                writer.WriteSafeElementString("instant", "yes");
            }

            // Writes <format>...</format
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
