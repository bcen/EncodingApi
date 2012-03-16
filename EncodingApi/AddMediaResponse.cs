using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EncodingApi
{
    [XmlRoot("response")]
    public class AddMediaResponse : BasicResponse
    {
        [XmlElement("MediaID")]
        public string MediaId { get; set; }

        public AddMediaResponse()
        {
        }
    }
}
