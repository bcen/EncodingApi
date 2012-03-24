using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace EncodingApi
{
    public partial class EncodingServiceClient : BasicXmlWebServiceClient
    {
        /// <summary>
        /// The default host URI.
        /// </summary>
        public override Uri DefaultHost 
        { get { return new Uri("http://manage.encoding.com/"); } }

        /// <summary>
        /// The host URI to use when SSL is enabled.
        /// </summary>
        public override Uri DefaultSslHost 
        { get { return new Uri("https://manage.encoding.com/"); } }

        /// <summary>
        /// Encoding.com's user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Encoding.com's user key.
        /// </summary>
        public string UserKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the EncodingWebRequest class.
        /// </summary>
        public EncodingServiceClient()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the EncodingServiceClient class for the specified user key
        /// and user id.
        /// </summary>
        /// <param name="uid">The 3 to 5 digits user id.</param>
        /// <param name="ukey">The user key.</param>
        public EncodingServiceClient(string uid, string ukey)
            : base()
        {
            UserId = uid;
            UserKey = ukey;
        }

        public virtual T GetResponse<T>(EncodingQuery query) where T : class, new()
        {
            if (UserId == null || UserKey == null)
                throw new EncodingServiceException("UserId or UserKey is empty");

            query.UserId = UserId;
            query.UserKey = UserKey;

            string xml = String.Empty;
            try
            {
                xml = GetXmlResponse(Serialize(query));
            }
            catch (WebException ex)
            {
                throw new EncodingServiceException(ex.Message, ex);
            }
            return Deserialize<T>(xml);
        }

        /// <summary>
        /// Requests a list of media meta from server.
        /// </summary>
        /// <returns>A collection of GetMediaListResponse.Media.</returns>
        public ICollection<GetMediaListResponse.Media> GetMediaList()
        {
            var result = GetResponse<GetMediaListResponse>(EncodingQuery.CreateGetMediaListQuery());
            if (result.Errors.Count > 0)
            {
                string message = result.Errors.First();
                EncodingServiceException ex = new EncodingServiceException(message);
                ex.Data.Add("errors", result.Errors);
                throw ex;
            }
            return result.MediaList;
        }

        /// <summary>
        /// Adds media from the specified sources with the formats to the server for encoding.
        /// </summary>
        /// <param name="sources">List of Uri of source to be added for endcoding.</param>
        /// <param name="formats">List of format query for the sources.</param>
        /// <param name="isInstant">To test whether the process should start immediately or not.
        /// </param>
        /// <param name="notifyUri">
        /// The xml callback Uri address for notification. 
        /// It could be a valid email mailto address.
        /// </param>
        /// <returns>The added media id.</returns>
        public string AddMedia(IEnumerable<Uri> sources, IEnumerable<EncodingFormat> formats,
                               bool isInstant=false, Uri notifyUri=null)
        {
            if (sources == null || formats == null)
                throw new ArgumentNullException("sources or formats cannot be null.");

            EncodingQuery query = new EncodingQuery();
            query.Action = EncodingQuery.QueryAction.AddMedia;
            query.Formats = formats.ToList();
            query.Notify = notifyUri;
            foreach (Uri uri in sources)
            {
                query.Sources.Add(uri);
            }
            query.IsInstant = isInstant;
            
            var result = GetResponse<AddMediaResponse>(query);
            if (result.Errors.Count > 0)
            {
                string message = result.Errors.First();
                EncodingServiceException ex = new EncodingServiceException(message);
                ex.Data.Add("errors", result.Errors);
                throw ex;
            }

            return result.MediaId;
        }
    }
}
