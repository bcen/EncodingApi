using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using EncodingApi.Models;

namespace EncodingApi
{
    public class EncodingWebRequest
    {
        /// <summary>
        /// The default host URI.
        /// </summary>
        public static readonly Uri DefaultHost = new Uri("http://manage.encoding.com/");

        /// <summary>
        /// The host URI to use when SSL is enabled.
        /// </summary>
        public static readonly Uri DefaultSslHost = new Uri("https://manage.encoding.com/");

        /// <summary>
        /// Encoding.com's user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Encoding.com's user key.
        /// </summary>
        public string UserKey { get; set; }

        private HttpWebRequest request;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EncodingWebRequest()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes the object with user id and user key. Internally it initializes a
        /// HttpWebRequest object without SSL.
        /// </summary>
        /// <param name="uid">The 3 to 5 digits user id.</param>
        /// <param name="ukey">The user key.</param>
        public EncodingWebRequest(string uid, string ukey)
        {
            UserId = uid;
            UserKey = ukey;
            InitializeHttpWebRequest(false);
        }

        /// <summary>
        /// Forces to use SSL. It will recreate a new HttpWebRequest object, thus previous settings
        /// will be lost.
        /// </summary>
        public void EnableSslConnection()
        {
            InitializeHttpWebRequest(true);
        }

        /// <summary>
        /// Disables SSL. It will recreate a new HttpWebRequest object, thus previous settings
        /// will be lost.
        /// </summary>
        public void DisableSslConnection()
        {
            InitializeHttpWebRequest(false);
        }

        /// <summary>
        /// Sends a GetMediaList request to the server.
        /// </summary>
        /// <returns>An instance of GetMediaListResponse.</returns>
        public GetMediaListResponse SendGetMediaListRequest()
        {
            string result = SendRequest(EncodingApiQuery.CreateGetMediaListQuery());
            return new GetMediaListResponse(result);
        }

        /// <summary>
        /// Internally it sends a GetMediaList request to the server and extract a list
        /// of GetMediaListResponse.Media from the GetMediaListResponse.
        /// </summary>
        /// <returns>A collection of GetMediaListResponse.Media.</returns>
        public ICollection<GetMediaListResponse.Media> GetMediaList()
        {
            var result = SendGetMediaListRequest();
            if (result.Errors.Count > 0)
            {
                string message = result.Errors.First();
                EncodingWebRequestException ex = new EncodingWebRequestException(message);
                ex.Data.Add("errors", result.Errors);
                throw ex;
            }

            return result.MediaList;
        }
        
        private string SendRequest(EncodingApiQuery qry)
        {
            if (UserId == null || UserKey == null)
                throw new Exception("UserId or UserKey is empty");

            string xmlResult = "<nothing/>";
            qry.UserId = UserId;
            qry.UserKey = UserKey;
            
            string content = "xml=" + qry.ToString();
            qry = null;

            if (request != null)
            {
                request.ContentLength = content.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(Encoding.UTF8.GetBytes(content), 0, content.Length);
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(),
                                                                  Encoding.UTF8))
                    {
                        xmlResult = reader.ReadToEnd();
                    }
                }
            }

            return xmlResult;
        }

        private void InitializeHttpWebRequest(bool useSsl)
        {
            Uri reqUri = useSsl ? DefaultSslHost : DefaultHost;
            request = WebRequest.Create(reqUri) as HttpWebRequest;
            if (request != null)
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
            }
        }

        /// <summary>
        /// Gets and sets the time out duration for the web request connection.
        /// </summary>
        public int Timeout
        {
            get
            {
                return request.Timeout;
            }
            set
            {
                request.Timeout = value;
            }
        }
    }
}
