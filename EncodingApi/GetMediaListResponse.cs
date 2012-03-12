using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EncodingApi
{
    public class GetMediaListResponse : ResponseBase
    {
        private ICollection<Media> _mediaList;

        public GetMediaListResponse()
            : this("<response/>")
        {
        }

        public GetMediaListResponse(string xml)
            : base(xml)
        {
        }

        private IList<Media> GetMediaList()
        {
            var rawMediaList = from node in Root.Elements("media")
                               where
                               (
                                   node.Element("mediafile") != null &&
                                   node.Element("mediaid") != null &&
                                   node.Element("mediastatus") != null &&
                                   node.Element("createdate") != null &&
                                   node.Element("startdate") != null &&
                                   node.Element("finishdate") != null
                               )
                               select new
                               {
                                   mfile = node.Element("mediafile").Value,
                                   mid = node.Element("mediaid").Value,
                                   mstatus = node.Element("mediastatus").Value,
                                   cdate = node.Element("createdate").Value,
                                   sdate = node.Element("startdate").Value,
                                   fdate = node.Element("finishdate").Value
                               };

            Media m;
            IList<Media> mediaList = new List<Media>();
            DateTime d;
            foreach (var v in rawMediaList)
            {
                m = new Media();

                m.MediaFile = new Uri(v.mfile);
                m.MediaId = v.mid;
                m.MediaStatus = v.mstatus;

                if (DateTime.TryParse(v.cdate, out d))
                {
                    m.CreateDate = new DateTime(d.Ticks);
                }
                if (DateTime.TryParse(v.sdate, out d))
                {
                    m.StartDate = new DateTime(d.Ticks);
                }
                if (DateTime.TryParse(v.fdate, out d))
                {
                    m.FinishDate = new DateTime(d.Ticks);
                }

                mediaList.Add(m);
                m = null;
            }

            rawMediaList = null;
            return mediaList;
        }

        public ICollection<Media> MediaList
        {
            get
            {
                if (_mediaList == null)
                {
                    _mediaList = new ReadOnlyCollection<Media>(GetMediaList());
                }

                return _mediaList;
            }
        }

        public class Media
        {
            public Uri MediaFile { get; set; }
            public string MediaId { get; set; }
            public string MediaStatus { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime FinishDate { get; set; }

            public Media()
            {
            }

            public Media(Uri mediaFile, string mid, string mediaStatus, DateTime createDate,
                         DateTime startDate, DateTime finishDate)
            {
                MediaFile = mediaFile;
                MediaId = mid;
                MediaStatus = mediaStatus;
                CreateDate = createDate;
                StartDate = startDate;
                FinishDate = finishDate;
            }
        }
    }
}
