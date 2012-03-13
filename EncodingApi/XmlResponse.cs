using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EncodingApi
{
    public abstract class XmlResponse : Models.XmlModelBase
    {
        private string _message;
        private ICollection<string> _errors;

        public XmlResponse()
            : this("<response/>")
        {
        }

        public XmlResponse(string xml)
            : base(xml)
        {
        }

        private IList<string> GetErrors()
        {
            var result = from node in Root.Descendants("error")
                         select node.Value;

            return result.ToList<string>();
        }

        public string Message
        {
            get
            {
                if (String.IsNullOrEmpty(_message))
                {
                    string text = GetXmlElementInnerText("message");
                    _message = String.IsNullOrEmpty(text) ? String.Empty : text;
                }
                return _message;
            }
        }

        public ICollection<string> Errors
        {
            get
            {
                if (_errors == null)
                {
                    _errors = new ReadOnlyCollection<string>(GetErrors());
                }
                return _errors;
            }
        }
    }
}
