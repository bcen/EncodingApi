using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace EncodingApi
{
    public class EncodingWebRequest
    {
        public static readonly Uri DefaultHost = new Uri("http://manage.encoding.com/");
        public static readonly Uri DefaultSslHost = new Uri("https://manage.encoding.com/");
        public string UserId { get; set; }
        public string UserKey { get; set; }
        private HttpWebRequest request;

        public EncodingWebRequest()
            : this(null, null)
        {
        }

        public EncodingWebRequest(string uid, string ukey)
        {
            UserId = uid;
            UserKey = ukey;
            InitializeHttpWebRequest(false);
        }

        public void EnableSslConnection()
        {
            InitializeHttpWebRequest(true);
        }

        public void DisableSslConnection()
        {
            InitializeHttpWebRequest(false);
        }

        public void SendGetMediaListRequestAsync<T>(Action<T> callback)
        {
            // TODO: Send request async.
        }

        public void SendAddMediaRequest(Uri[] sources)
        {
            EncodingApiQuery qry = new EncodingApiQuery();
            qry.AddMultipleSource(sources);
            string result = SendRequest(qry);
        }

        public GetMediaListResponse SendGetMediaListRequest()
        {
            string result = SendRequest(EncodingApiQuery.CreateGetMediaListQuery());
            return new GetMediaListResponse(result);
        }

        public ICollection<GetMediaListResponse.Media> GetMediaList()
        {
            var result = SendGetMediaListRequest();
            if (result.Errors.Count > 0)
            {
                string errorMessage = result.Errors.First();
                throw new Exception(errorMessage);
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
