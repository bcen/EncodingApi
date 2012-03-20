using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EncodingApi
{
    /// <summary>
    /// A common XML web service api client abstract base class.
    /// </summary>
    public abstract class BasicXmlWebServiceClient
    {
        /// <summary>
        /// The default host.
        /// </summary>
        public abstract Uri DefaultHost { get; }

        /// <summary>
        /// The default host to use when SSL is enabled.
        /// Sets UseSslConnection to true to enable SSL connection.
        /// </summary>
        public abstract Uri DefaultSslHost { get; }

        /// <summary>
        /// Gets and sets the timeout of the connection.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets and sets the proxy.
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Gets and sets the option to use SSL.
        /// </summary>
        public bool UseSslConnection { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BasicXmlWebServiceClient()
        {
            Timeout = 100000; // Default value from HttpWebRequest.Timeout
            UseSslConnection = true;
        }

        /// <summary>
        /// Creates a HttpWebRequest for that used in GetXmlResponse and GetXmlResponseAsync.
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

        /// <summary>
        /// Gets the response in xml string format.
        /// </summary>
        /// <param name="xmlRequestString">The xml request string for the parameter: "xml".</param>
        /// <returns>The xml response string.</returns>
        protected virtual string GetXmlResponse(string xmlRequestString)
        {
            // Default xmlResponse if nothing is read from the response.
            string xmlResponse = "<response><error>Cannot establish a request to the server</error></response>";

            HttpWebRequest request = CreateRequest(UseSslConnection ? DefaultSslHost : DefaultHost);
            if (request != null)
            {
                string content = "xml=" + xmlRequestString;
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
                    throw ex;
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
                        xmlResponse = reader.ReadToEnd();
                    }
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }
            }

            return xmlResponse;

        }

        /// <summary>
        /// Gets response asynchronously.
        /// </summary>
        /// <param name="xmlRequestString">The xml request string.</param>
        /// <param name="callback">The callback for xml result.</param>
        protected virtual void GetXmlResponseAsync(string xmlRequestString, Action<string> callback)
        {
            // Default xmlResult if nothing is read from the response.
            string xmlResponse = "<response><error>Cannot establish a request to the server</error></response>";
            string content = "xml=" + xmlRequestString;

            // Setup request.
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
                            xmlResponse = reader.ReadToEnd();
                        }
                    }


                    // callback with the xml result.
                    callback(xmlResponse);
                }, null);
            }, null);
        }

        /// <summary>
        /// Serializes the object into xml string.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The xml string.</returns>
        public virtual string Serialize<T>(T obj) where T : class, new()
        {
            return Serialize<T>(obj, null);
        }

        /// <summary>
        /// Serializes the object into xml string with the specified indentation characters.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="indentChars">The indentation string.</param>
        /// <returns>The xml string.</returns>
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

        /// <summary>
        /// Deserializes the xml string into object with the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the return object.</typeparam>
        /// <param name="xml">The xml string to be deserialized.</param>
        /// <returns>The object with the specified type that represents the xml string.</returns>
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
