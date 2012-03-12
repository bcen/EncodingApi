using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EncodingApi.Models
{
    /// <summary>
    /// A more object oriented way to construct a xml query for http://www.encoding.com.
    /// </summary>
    public class EncodingApiQuery : XmlModelBase
    {
        public EncodingApiQuery()
            : this("<query/>")
        {
        }

        public EncodingApiQuery(string xml)
            : base(xml)
        {
        }

        public static EncodingApiQuery CreateGetMediaListQuery()
        {
            EncodingApiQuery qry = new EncodingApiQuery();
            qry.Action = EncodingApiQuery.QueryAction.GetMediaList;
            return qry;
        }

        /// <summary>
        /// Add multiple source files for AddMedia and AddMediaBechmark actions.
        /// </summary>
        /// <param name="uri">A uri to the source.</param>
        /// <returns>A instance of this class for method chaining.</returns>
        public EncodingApiQuery AddMultipleSource(IEnumerable<Uri> sourceList)
        {
            if (sourceList == null)
                throw new ArgumentNullException("sourceList");
            
            foreach (Uri uri in sourceList)
            {
                AddSource(uri);
            }
            return this;
        }

        /// <summary>
        /// Add a source file for AddMedia and AddMediaBechmark actions.
        /// </summary>
        /// <remarks>
        /// Must be specified only for AddMedia and AddMediaBenchmark actions.
        /// </remarks>
        /// <param name="uri">A uri to the source.</param>
        /// <returns>A instance of this class for method chaining.</returns>
        public EncodingApiQuery AddSource(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");
            
            Root.Add(new XElement("source", uri.AbsoluteUri));
            return this;
        }

        /// <summary>
        /// Gets a IList of all the sources to be processed.
        /// </summary>
        /// <returns>A <code>List</code> of Uri of all the sources in the query.</returns>
        public IList<Uri> GetAllSources()
        {
            var s = from x in Root.Elements("source")
                    select new Uri(x.Value);
            return s.ToList<Uri>();
        }

        /// <summary>
        /// Removes all sources.
        /// </summary>
        public void RemoveAllSources()
        {
            Root.Elements("source").Remove();
        }

        /// <summary>
        /// Add multiple format into the query.
        /// </summary>
        /// <param name="formatList">A collection of <code>EncodingFormat</code>.</param>
        /// <returns></returns>
        public EncodingApiQuery AddMultipleFormats(IEnumerable<EncodingFormat> formatList)
        {
            if (formatList == null)
                throw new ArgumentNullException("formatList");

            foreach (EncodingFormat f in formatList)
            {
                AddFormat(f);
            }
            return this;
        }

        public EncodingApiQuery AddFormat(EncodingFormat format)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            Root.Add(XElement.Parse(format.ToString()));
            return this;
        }

        public IList<EncodingFormat> GetAllFormats()
        {
            var s = from x in Root.Elements("format")
                    select new EncodingFormat(ToString(x, false));
            return s.ToList();
        }

        public void RemoveAllFormats()
        {
            Root.Elements("format").Remove();
        }

        /// <summary>
        /// Gets or sets the unique user identifier id of the query.
        /// <remarks>
        /// This 3-5 digits number can be found in the My Account tab of the Client Interface.
        /// </remarks>
        /// </summary>
        public string UserId
        {
            get
            {
                return GetXmlElementInnerText("userid");
            }
            set
            {
                SetXmlElementInnerText("userid", value);
            }
        }

        /// <summary>
        /// Gets or sets the unique authentication key of the query.
        /// <remarks>
        /// Created automatically when a user is created and can be regenerated at anytime in the
        /// My Account tab of the Client Interface.
        /// </remarks>
        /// </summary>
        public string UserKey
        {
            get
            {
                return GetXmlElementInnerText("userkey");
            }
            set
            {
                SetXmlElementInnerText("userkey", value);
            }
        }

        /// <summary>
        /// Gets or sets the action to be performed in the API request.
        /// Use EncodingApiQuery.QueryAction to look up the availiable actions.
        /// </summary>
        public string Action
        {
            get
            { 
                return GetXmlElementInnerText("action"); 
            }
            set
            { 
                SetXmlElementInnerText("action", value); 
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier for each media.
        /// Required for UpdateMedia, CancelMedia and GetStatus action.
        /// <remarks>
        /// A unique identifier for each media. This field must be specified for the following
        /// actions: UpdateMedia, CancelMedia, GetStatus.
        /// </remarks>
        /// </summary>
        public string MediaId
        {
            get
            { 
                return GetXmlElementInnerText("mediaid"); 
            }
            set
            { 
                SetXmlElementInnerText("mediaid", value); 
            }
        }
        
        /// <summary>
        /// Set to true to initiate the encoding process immediately when source video begins 
        /// downloading to the processing center as opposed to waiting until after the download
        /// has completed. Also, this feature can be used when source media is still uploading 
        /// to the specified source FTP location - the system will recognize if the source file 
        /// size increases while downloading, or soon after, and the "tail" will be downloaded 
        /// and concatenated.
        /// </summary>
        public bool IsInstant
        {
            get
            {
                string text = GetXmlElementInnerText("instant");
                if (String.IsNullOrEmpty(text))
                {
                    return false;
                }
                return text.Trim().ToLower().Equals("yes");
            }
            set
            { 
                SetXmlElementInnerText("instant", value ? "yes" : "no");
            }
        }

        /// <summary>
        /// Can be either an HTTP(S) URL for the script with which the result will be posted, or 
        /// a mailto: link with email address for which the result info will be sent. This field
        /// may be specified for AddMedia and AddMediaBenchmark actions.
        /// </summary>
        public Uri Notify
        {
            get
            { 
                string text = GetXmlElementInnerText("notify");
                Uri uri = String.IsNullOrEmpty(text) ? null : new Uri(text);
                return uri; 
            }
            set
            {
                SetXmlElementInnerText("notify", value == null ? null : value.AbsoluteUri);
            }
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
