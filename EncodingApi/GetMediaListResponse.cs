using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace EncodingApi
{
    /// <summary>
    /// Encapsulates GetMediaList XML response.
    /// </summary>
    [XmlRoot("response")]
    public class GetMediaListResponse : BasicResponse
    {
        /// <summary>
        /// List of media meta from server.
        /// </summary>
        [XmlElement("media")]
        public List<Media> MediaList { get; set; }

        /// <summary>
        /// To test wether to serialize MediaList or not.
        /// </summary>
        /// <returns>True if media list count is greater than zero, otherwise false.</returns>
        public bool ShouldSerializeMediaList() { return (MediaList.Count > 0); }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GetMediaListResponse()
        {
            MediaList = new List<Media>();
        }

        /// <summary>
        /// Media meta class.
        /// </summary>
        public class Media
        {
            /// <summary>
            /// The source URL of the media.
            /// </summary>
            [XmlElement("mediafile")]
            public string MediaFile { get; set; }

            /// <summary>
            /// The ID of the media.
            /// </summary>
            [XmlElement("mediaid")]
            public string MediaId { get; set; }
            
            /// <summary>
            /// The status of the media.
            /// </summary>
            [XmlElement("mediastatus")]
            public string MediaStatus { get; set; }

            /// <summary>
            /// The date when the media is first created in the queue.
            /// </summary>
            [XmlElement("createdate")]
            public string CreateDate { get; set; }

            /// <summary>
            /// The date when the media is first started in the processing queue.
            /// </summary>
            [XmlElement("startdate")]
            public string StartDate { get; set; }

            /// <summary>
            /// The date when the media is finished.
            /// </summary>
            [XmlElement("finishdate")]
            public string FinishDate { get; set; }

            /// <summary>
            /// To test whether serialize MediaId or not.
            /// </summary>
            /// <returns>True if MediaId is not null nor empty string, otherwise false.</returns>
            public bool ShouldSerializeMediaId() { return !String.IsNullOrEmpty(MediaId); }

            /// <summary>
            /// To test whether serialize MediaStatus or not.
            /// </summary>
            /// <returns>True if MediaStatus is not null nor empty string, otherwise false.</returns>
            public bool ShouldSerializeMediaStatus() { return !String.IsNullOrEmpty(MediaStatus); }

            /// <summary>
            /// Default constructor.
            /// </summary>
            public Media()
            {
                MediaId = String.Empty;
                MediaStatus = String.Empty;
            }

            /// <summary>
            /// Converts MediaFile URL string to Uri.
            /// </summary>
            /// <returns>An instance of Uri class.</returns>
            public Uri GetMediaFileUri()
            {
                Uri uri = null;
                if (!String.IsNullOrEmpty(MediaFile))
                {
                    uri = new Uri(MediaFile);
                }
                return uri;
            }

            /// <summary>
            /// Converts CreateDate string representation to DateTime.
            /// </summary>
            /// <returns>An instance of DateTime for CreateDate.</returns>
            public DateTime GetCreateDate()
            {
                DateTime d = DateTime.MinValue;
                if (!String.IsNullOrEmpty(CreateDate))
                {
                    DateTime.TryParse(CreateDate, out d);
                }
                return d;
            }

            /// <summary>
            /// Converts StartDate string representation to DateTime.
            /// </summary>
            /// <returns>An instance of DateTime for StartDate.</returns>
            public DateTime GetStartDate()
            {
                DateTime d = DateTime.MinValue;
                if (!String.IsNullOrEmpty(StartDate))
                {
                    DateTime.TryParse(StartDate, out d);
                }
                return d;
            }

            /// <summary>
            /// Converts FinishDate string representation to DateTime.
            /// </summary>
            /// <returns>An instance of DateTime for FinishDate.</returns>
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
