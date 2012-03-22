using System;
using System.Xml.Serialization;

namespace EncodingApi
{
    /// <summary>
    /// A simple XML serializable Uri wrapper.
    /// </summary>
    public class UriWrapper
    {
        private Uri _uri;

        public UriWrapper()
        {
        }

        public UriWrapper(string uriString)
        {
            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
            {
                uriString = Uri.EscapeUriString(uriString);
            }
            Uri.TryCreate(uriString, UriKind.Absolute, out _uri);
        }

        [XmlIgnore]
        public Uri InternalUri { get { return _uri; } }

        [XmlIgnore]
        public string AbsolutePath
        { get { return (_uri != null) ? _uri.AbsolutePath : String.Empty; } }

        [XmlText]
        public string AbsoluteUri
        {
            get { return (_uri != null) ? _uri.AbsoluteUri : String.Empty; }
            set
            {
                string uriString = value;
                if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    uriString = Uri.EscapeUriString(uriString);
                }
                _uri = null;
                Uri.TryCreate(uriString, UriKind.Absolute, out _uri);
            }
        }

        [XmlIgnore]
        public string Authority
        { get { return (_uri != null) ? _uri.Authority : String.Empty; } }

        [XmlIgnore]
        public string DnsSafeHost
        { get { return (_uri != null) ? _uri.DnsSafeHost : String.Empty; } }
    }
}

