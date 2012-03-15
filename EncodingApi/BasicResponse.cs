using System.Collections.Generic;
using System.Xml.Serialization;

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
        }

        public bool ShouldSerializeErrors()
        {
            return (Errors.Count > 0);
        }
    }
}
