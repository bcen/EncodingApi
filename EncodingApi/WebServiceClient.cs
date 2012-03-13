using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using EncodingApi.Models;

namespace EncodingApi
{
    public class WebServiceClient
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

        /// <summary>
        /// Gets and sets the option to use SSL.
        /// </summary>
        public bool UseSsl { get; set; }

        /// <summary>
        /// Gets and sets the timeout of the connection.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets and sets the proxy.
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Initializes a new instance of the EncodingWebRequest class.
        /// </summary>
        public WebServiceClient()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the EncodingWebRequest class for the specified user key
        /// and user id.
        /// </summary>
        /// <param name="uid">The 3 to 5 digits user id.</param>
        /// <param name="ukey">The user key.</param>
        public WebServiceClient(string uid, string ukey)
        {
            UserId = uid;
            UserKey = ukey;
            Timeout = 100000;
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

        public void SendGetMediaListRequestAsync(Action<GetMediaListResponse> callback)
        {
            throw new NotImplementedException("Come back later");
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
                throw new EncodingWebRequestException("UserId or UserKey is empty");

            string xmlResult = "<response><error>Cannot establish a request to the server</error></response>";
            HttpWebRequest request = CreateWebRequest();

            if (request != null)
            {
                qry.UserId = UserId;
                qry.UserKey = UserKey;

                string content = "xml=" + qry.ToString();
                qry = null;

                request.ContentLength = content.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(Encoding.UTF8.GetBytes(content), 0, content.Length);
                }

                using (StreamReader reader = new StreamReader(request.GetResponse()
                                                                     .GetResponseStream()))
                {
                    xmlResult = reader.ReadToEnd();
                }
            }

            return xmlResult;
        }

        private HttpWebRequest CreateWebRequest()
        {
            Uri host = UseSsl ? DefaultSslHost : DefaultHost;
            HttpWebRequest request = WebRequest.Create(host) as HttpWebRequest;
            if (request != null)
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = Timeout;
                request.Proxy = Proxy;
            }
            return request;
        }
    }
}
