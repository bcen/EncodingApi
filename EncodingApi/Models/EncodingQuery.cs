using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace EncodingApi.Models
{
    /// <summary>
    /// A object oriented way to construct a xml query for http://www.encoding.com.
    /// </summary>
    [XmlRoot("query")]
    public class EncodingQuery
    {
        /// <summary>
        /// A unique user identifier.
        /// </summary>
        [XmlElement("userid")]
        public string UserId { get; set; }

        /// <summary>
        /// User's unique authentication key string.
        /// </summary>
        [XmlElement("userkey")]
        public string UserKey { get; set; }

        /// <summary>
        /// The action to be performed in the API request.
        /// </summary>
        [XmlElement("action")]
        public string Action { get; set; }

        /// <summary>
        /// A unique identifier for each media. 
        /// This field must be specified for the following actions: UpdateMedia, CancelMedia, 
        /// GetStatus.
        /// </summary>
        [XmlElement("mediaid")]
        public string MediaId { get; set; }

        /// <summary>
        /// Source media file. Must be specified only for AddMedia and AddMediaBenchmark actions.
        /// Always use AddSourceUri(Uri) to have proper encoded URL string.
        /// </summary>
        [XmlElement("source")]
        public List<string> Sources { get; set; }

        /// <summary>
        /// Can be either an HTTP(S) URL for the script with which the result will be posted,
        /// or a mailto: link with email address for which the result info will be sent. 
        /// This field may be specified for AddMedia and AddMediaBenchmark actions.
        /// SetNotifyUri() will sanitize the url string.
        /// </summary>
        [XmlElement("notify")]
        public string Notify { get; set; }

        /// <summary>
        /// Set to 'yes' to initiate the encoding process immediately when source video begins
        /// downloading to our processing center as opposed to waiting until after the
        /// download has completed. Also, this feature can be used when source media is
        /// still uploading to the specified source FTP location - our system will recognize if
        /// the source file size increases while downloading, or soon after, and the "tail" will
        /// be downloaded and concatenated.
        /// </summary>
        [XmlElement("instant")]
        public string Instant { get; set; }

        /// <summary>
        /// One or more format elements are required for AddMedia and UpdateMedia actions.
        /// </summary>
        [XmlElement("format")]
        public List<EncodingFormat> Formats { get; set; }

        /// <summary>
        /// To test whether to serialize UserId or not.
        /// </summary>
        /// <returns>True if UserId is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeUserId() { return !String.IsNullOrEmpty(UserId); }

        /// <summary>
        /// To test whether to serialize UserKey or not.
        /// </summary>
        /// <returns>True if UserKey is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeUserKey() { return !String.IsNullOrEmpty(UserId); }

        /// <summary>
        /// To test whether to serialize Action or not.
        /// </summary>
        /// <returns>True if Action is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeAction() { return !String.IsNullOrEmpty(Action); }

        /// <summary>
        /// To test whether to serialize MediaId or not.
        /// </summary>
        /// <returns>True if MediaId is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeMediaId() { return !String.IsNullOrEmpty(MediaId); }

        /// <summary>
        /// To test whether to serialize Sources or not.
        /// </summary>
        /// <returns>True if Sources count is greater than zero, otherwise false.</returns>
        public bool ShouldSerializeSources() { return (Sources.Count > 0); }

        /// <summary>
        /// To test whether to serialize Notify or not.
        /// </summary>
        /// <returns>True if Notify is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeNotify() { return !String.IsNullOrEmpty(Notify); }

        /// <summary>
        /// To test whether to serialize Instant or not.
        /// </summary>
        /// <returns>True if Instant is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeInstant() { return !String.IsNullOrEmpty(Instant); }

        /// <summary>
        /// To test whether to serialize Formats or not.
        /// </summary>
        /// <returns>True if Formats count is greater than zero, otherwise false.</returns>
        public bool ShouldSerializeFormats() { return (Formats.Count > 0); }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EncodingQuery()
        {
            UserId = String.Empty;
            UserKey = String.Empty;
            Action = String.Empty;
            MediaId = String.Empty;
            Notify = String.Empty;
            Instant = String.Empty;
            Sources = new List<string>();
            Formats = new List<EncodingFormat>();
        }

        /// <summary>
        /// Creates a complete query for action: GetMediaList.
        /// </summary>
        /// <returns>A complete query for GetMediaList.</returns>
        public static EncodingQuery CreateGetMediaListQuery()
        {
            EncodingQuery qry = new EncodingQuery();
            qry.Action = EncodingQuery.QueryAction.GetMediaList;
            return qry;
        }

        /// <summary>
        /// Adds source URL to the query.
        /// </summary>
        /// <param name="uri">The URL to be added.</param>
        public void AddSourceUri(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri cannot be null.");

            Sources.Add(uri.AbsoluteUri);
        }

        /// <summary>
        /// Adds source URL to the query.
        /// </summary>
        /// <param name="uriString">The URL string to be added.</param>
        public void AddSourceUri(string uriString)
        {
            if (uriString == null)
                throw new ArgumentNullException("uriString cannot be null.");

            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
            {
                uriString = Uri.EscapeUriString(uriString);
            }

            Uri uri = null;
            if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
            {
                AddSourceUri(uri);
            }
        }

        /// <summary>
        /// Gets all sources URL.
        /// </summary>
        /// <returns>List of URL in the query.</returns>
        public IEnumerable<Uri> GetAllSourceUri()
        {
            var rawList = from rawUri in Sources
                          select new Uri(rawUri);
            return rawList;
        }

        /// <summary>
        /// Removes all URL in the query that matches the specified URL.
        /// </summary>
        /// <param name="uri">The matching URL to be removed.</param>
        public void RemoveSourceUri(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri cannot be null.");

            Sources.RemoveAll((rawUri) =>
            {
                return (uri.AbsoluteUri.Equals(rawUri));
            });
        }

        /// <summary>
        /// Returns a Notify Uri.
        /// </summary>
        /// <returns>An instance of Uri class.</returns>
        public Uri GetNotifyUri()
        {
            Uri uri = null;
            Uri.TryCreate(Notify, UriKind.Absolute, out uri);
            return uri;
        }

        /// <summary>
        /// Sets the Notify string property with the specified Uri.
        /// </summary>
        /// <param name="newUri">The new Uri to be setted.</param>
        public void SetNotifyUri(Uri newUri)
        {
            Notify = (newUri == null) ? String.Empty : newUri.AbsoluteUri;
        }

        /// <summary>
        /// Sets the Notify string property with the specified uriString.
        /// </summary>
        /// <param name="uriString">The new Uri to be setted.</param>
        public void SetNotifyUri(string uriString)
        {
            if (uriString == null)
            {
                SetNotifyUri((Uri)null);
                return;
            }

            // If the uriString is not well formed, tries to escape the string.
            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
            {
                uriString = Uri.EscapeUriString(uriString);
            }

            Uri uri = null;
            if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
            {
                SetNotifyUri(uri);
            }
        }

        /// <summary>
        /// To test if the Instant property is set to yes or no.
        /// </summary>
        /// <returns>True if Instant == "yes", otherwise false.</returns>
        public bool IsInstant()
        {
            return String.IsNullOrEmpty(Instant) ? false : (Instant.ToLower().StartsWith("yes"));
        }

        /// <summary>
        /// Sets Instant to be "yes".
        /// </summary>
        public void TurnOnInstantProcess()
        {
            Instant = "yes";
        }

        /// <summary>
        /// Sets Instant to be String.Empty, which tells the xml serializer not to serialize
        /// this property.
        /// </summary>
        public void TurnOffInstantProcess()
        {
            Instant = String.Empty;
        }

        /// <summary>
        /// A convenience class to look up availiable query actions.
        /// </summary>
        public static class QueryAction
        {
            public readonly static string AddMedia           = "AddMedia";
            public readonly static string AddMediaBenchmark  = "AddMediaBenchmark";
            public readonly static string UpdateMedia        = "UpdateMedia";
            public readonly static string ProcessMedia       = "ProcessMedia";
            public readonly static string CancelMedia        = "CancelMedia";
            public readonly static string GetMediaList       = "GetMediaList";
            public readonly static string GetStatus          = "GetStatus";
            public readonly static string GetMediaInfo       = "GetMediaInfo";
            public readonly static string RestartMedia       = "RestartMedia";
            public readonly static string RestartMediaErrors = "RestartMediaErrors";
            public readonly static string RestartMediaTask   = "RestartMediaTask";
        }
    }
}
