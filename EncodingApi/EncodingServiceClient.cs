using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using EncodingApi.Models;

namespace EncodingApi
{
    public class EncodingServiceClient
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
        public bool UseSslConnection { get; set; }

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
        public EncodingServiceClient()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the EncodingWebRequest class for the specified user key
        /// and user id.
        /// </summary>
        /// <param name="uid">The 3 to 5 digits user id.</param>
        /// <param name="ukey">The user key.</param>
        public EncodingServiceClient(string uid, string ukey)
        {
            UserId = uid;
            UserKey = ukey;
            Timeout = 100000; // Default value from HttpWebRequest.Timeout
            UseSslConnection = true;
        }

        /// <summary>
        /// Sends a GetMediaList request to the server.
        /// </summary>
        /// <returns>An instance of GetMediaListResponse.</returns>
        public GetMediaListResponse SendGetMediaListRequest()
        {
            string result = SendRequest(EncodingQuery.CreateGetMediaListQuery());
            return Deserialize<GetMediaListResponse>(result);
        }

        public AddMediaResponse SendAddMediaRequest(IEnumerable<Uri> sources, 
                                                    IEnumerable<EncodingFormat> formats)
        {
            EncodingQuery qry = new EncodingQuery();
            qry.Action = EncodingQuery.QueryAction.AddMedia;
            foreach (var uri in sources)
            {
                qry.AddSourceUri(uri);
            }
            qry.Formats = formats.ToList<EncodingFormat>();

            string xmlResult = SendRequest(qry);
            return Deserialize<AddMediaResponse>(xmlResult);
        }
        
        /// <summary>
        /// Sends a GetMediaList request asynchronously.
        /// </summary>
        /// <param name="callback">The callback to process the xml result.</param>
        public void SendGetMediaListRequestAsync(Action<GetMediaListResponse> callback)
        {
            SendRequestAsync(EncodingQuery.CreateGetMediaListQuery(), (xmlResult) =>
            {
                callback(Deserialize<GetMediaListResponse>(xmlResult));
            });
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
                EncodingServiceException ex = new EncodingServiceException(message);
                ex.Data.Add("errors", result.Errors);
                throw ex;
            }

            return result.MediaList;
        }

        public void GetMediaListAsync(Action<ICollection<GetMediaListResponse.Media>> callback,
                                      Action<ICollection<string>> errors)
        {
            SendGetMediaListRequestAsync((response) =>
            {
                callback(response.MediaList);
                errors(response.Errors);
            });
        }
        
        /// <summary>
        /// Sends a HttpWebRequest with EncodingQuery to Encoding.com and returns a 
        /// xml result.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A xml string.</returns>
        protected virtual string SendRequest(EncodingQuery query)
        {
            if (UserId == null || UserKey == null)
                throw new EncodingServiceException("UserId or UserKey is empty");

            // Default xmlResult if nothing is read from the response.
            string xmlResult = "<response><error>Cannot establish a request to the server</error></response>";
            
            HttpWebRequest request = CreateRequest(UseSslConnection ? DefaultSslHost : DefaultHost);
            if (request != null)
            {
                query.UserId = UserId;
                query.UserKey = UserKey;
                string content = "xml=" + Serialize(query);
                query = null;
                request.ContentLength = content.Length;

                // Writes EncodingQuery to the request stream.
                Stream stream = null;
                try
                {
                    stream = request.GetRequestStream();
                    stream.Write(Encoding.UTF8.GetBytes(content), 0, content.Length);
                }
                catch (WebException ex)
                {
                    throw new EncodingServiceException(ex.Message, ex);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }

                // Reads xml string from response using StreamReader.
                WebResponse response = null;
                try
                {
                    response = request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        xmlResult = reader.ReadToEnd();
                    }
                }
                catch (WebException ex)
                {
                    throw new EncodingServiceException(ex.Message, ex);
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }
            }

            return xmlResult;
        }

        /// <summary>
        /// Sends request asynchronously.
        /// </summary>
        /// <param name="query">The query for encoding service.</param>
        /// <param name="callback">The callback for xml result.</param>
        protected virtual void SendRequestAsync(EncodingQuery query, Action<string> callback)
        {
            if (UserId == null || UserKey == null)
                throw new EncodingServiceException("UserId or UserKey is empty");

            // Default xmlResult if nothing is read from the response.
            string xmlResult = "<response><error>Cannot establish a request to the server</error></response>";
            query.UserId = UserId;
            query.UserKey = UserKey;
            string content = "xml=" + Serialize(query);
            query = null;

            HttpWebRequest request = CreateRequest(UseSslConnection ? DefaultSslHost : DefaultHost);
            request.ContentLength = content.Length;

            // Begins the get request stream process.
            request.BeginGetRequestStream((requestStreamAsyncResult) =>
            {
                // Starts writing query to the stream.
                using (Stream stream = request.EndGetRequestStream(requestStreamAsyncResult))
                {
                    stream.Write(Encoding.UTF8.GetBytes(content), 0, content.Length);
                }

                // Begins the get response process.
                request.BeginGetResponse((responseAsyncResult) =>
                {
                    // Reads the stream and parse it to xml string.
                    using (WebResponse response = request.EndGetResponse(responseAsyncResult))
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            xmlResult = reader.ReadToEnd();
                        }
                    }


                    // callback with the xml result.
                    callback(xmlResult);
                }, null);
            }, null);
        }

        /// <summary>
        /// Creates a HttpWebRequest.
        /// </summary>
        /// <returns>A HttpWebRequest object.</returns>
        protected virtual HttpWebRequest CreateRequest(Uri host)
        {
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

        public virtual string Serialize<T>(T obj) where T : class, new()
        {
            return Serialize<T>(obj, null);
        }

        public virtual string Serialize<T>(T obj, string indentChars) where T : class, new()
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            StringBuilder sb = new StringBuilder();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            XmlWriterSettings settings = new XmlWriterSettings();

            if (!String.IsNullOrEmpty(indentChars))
            {
                settings.IndentChars = indentChars;
                settings.Indent = true;
            }
            ns.Add("", "");

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                ser.Serialize(writer, obj, ns);
            }

            if (sb.Length >= 37)
            {
                sb.Replace(Encoding.Unicode.WebName, Encoding.UTF8.WebName, 0, 37);
            }

            return sb.ToString();
        }

        public virtual T Deserialize<T>(string xml) where T : class, new()
        {
            T obj = default(T);
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(xml))
            {
                obj = (T)ser.Deserialize(reader);
            }

            return obj;
        }
    }
}
