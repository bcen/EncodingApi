using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace EncodingApi
{
    [XmlRoot("response")]
    public class GetMediaListResponse : BasicResponse
    {
        [XmlElement("media")]
        public List<Media> MediaList
        {
            get
            {
                return (_mediaList ?? (_mediaList = new List<Media>()));
            }
            set
            {
                _mediaList = value;
            }
        }
        private List<Media> _mediaList;

        public GetMediaListResponse()
        {
        }

        public class Media
        {
            [XmlElement("mediafile")]
            public string MediaFile { get; set; }

            [XmlElement("mediaid")]
            public string MediaId { get; set; }
            
            [XmlElement("mediastatus")]
            public string MediaStatus { get; set; }

            [XmlElement("createdate")]
            public string CreateDate { get; set; }

            [XmlElement("startdate")]
            public string StartDate { get; set; }

            [XmlElement("finishdate")]
            public string FinishDate { get; set; }

            public Media()
            {
            }

            public Uri GetMediaFileUri()
            {
                Uri uri = null;
                if (!String.IsNullOrEmpty(MediaFile))
                {
                    uri = new Uri(MediaFile);
                }
                return uri;
            }

            public DateTime GetCreateDate()
            {
                DateTime d = DateTime.MinValue;
                if (!String.IsNullOrEmpty(CreateDate))
                {
                    DateTime.TryParse(CreateDate, out d);
                }
                return d;
            }

            public DateTime GetStartDate()
            {
                DateTime d = DateTime.MinValue;
                if (!String.IsNullOrEmpty(StartDate))
                {
                    DateTime.TryParse(StartDate, out d);
                }
                return d;
            }

            public DateTime GetFinishDate()
            {
                DateTime d = DateTime.MinValue;
                if (!String.IsNullOrEmpty(FinishDate))
                {
                    DateTime.TryParse(FinishDate, out d);
                }
                return d;
            }
        }
    }
}
