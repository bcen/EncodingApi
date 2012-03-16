using System.Collections.Generic;
using System.Xml.Serialization;
using System;

namespace EncodingApi
{
    [XmlRoot("response")]
    public abstract class BasicResponse
    {
        [XmlElement("message")]
        public string Message { get; set; }

        [XmlArray("errors")]
        [XmlArrayItem("error")]
        public List<string> Errors
        {
            get
            {
                return (_errors ?? (_errors = new List<string>()));
            }
            set
            {
                _errors = value;
            }
        }
        private List<string> _errors;

        public BasicResponse()
        {
            Message = string.Empty;
        }

        public bool ShouldSerializeMessage()
        {
            return !String.IsNullOrEmpty(Message);
        }

        public bool ShouldSerializeErrors()
        {
            return (_errors != null);
        }
    }
}
