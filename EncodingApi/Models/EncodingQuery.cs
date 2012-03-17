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
        /// One or more format elements are required for AddMedia and UpdateMedia actions.
        /// </summary>
        [XmlElement("format")]
        public List<EncodingFormat> Formats { get; set; }
        
        [XmlElement("isinstant")]
        public bool? IsInstant { get; set; }

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
        /// To test whether to serialize Formats or not.
        /// </summary>
        /// <returns>True if Formats count is greater than zero, otherwise false.</returns>
        public bool ShouldSerializeFormats() { return (Formats.Count > 0); }

        /// <summary>
        /// To test whether to serialize IsInstance or not.
        /// </summary>
        /// <returns>True if IsInstant is assigned with a value, otherwise false.</returns>
        public bool ShouldSerializeIsInstant() { return IsInstant.HasValue; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EncodingQuery()
        {
            UserId = String.Empty;
            UserKey = String.Empty;
            Action = String.Empty;
            MediaId = String.Empty;
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
            Sources.Add(uri.AbsoluteUri);
        }

        /// <summary>
        /// Gets URL at the specified position.
        /// </summary>
        /// <param name="index">The 0-based position index.</param>
        /// <returns>An instance of Uri class.</returns>
        public Uri GetSourceUriAt(int index)
        {
            return new Uri(Sources[index]);
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
        /// Sets the source URL at the specified index.
        /// </summary>
        /// <param name="index">The 0-based position index.</param>
        /// <param name="newUri">The new URL to be setted.</param>
        public void SetSourceUriAt(int index, Uri newUri)
        {
            Sources[index] = newUri.AbsoluteUri;
        }

        /// <summary>
        /// Removes all URL in the query that matches the specified URL.
        /// </summary>
        /// <param name="uri">The matching URL to be removed.</param>
        public void RemoveSourceUri(Uri uri)
        {
            Sources.RemoveAll((rawUri) =>
            {
                return (uri.AbsoluteUri.Equals(rawUri));
            });
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
