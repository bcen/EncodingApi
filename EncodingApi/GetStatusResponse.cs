using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace EncodingApi
{
    public class GetStatusResponse : BasicResponse
    {
        public ICollection<MediaStatus> StatusList { get; set; }
    }

    public class MediaStatus : XmlSerializableObject
    {
        public string MediaId { get; set; }

        public string UserId { get; set; }

        public Uri SourceFile { get; set; }

        public string Status { get; set; }

        public Uri Notify { get; set; }

        public DateTime Created { get; set; }

        public DateTime Started { get; set; }

        public DateTime Finished { get; set; }

        public string PreviousStatus { get; set; }

        public DateTime Downloaded { get; set; }

        public DateTime Uploaded { get; set; }

        public string Description { get; set; }

        public long FileSize { get; set; }

        public string Processor { get; set; }

        public int TotalTimeLeft { get; set; }

        public double TotalProgress { get; set; }

        public int CurrentTimeLeft { get; set; }

        public double CurrentProgress { get; set; }

        public MediaStatus()
        {
            MediaId = String.Empty;
            UserId = String.Empty;
            Status = String.Empty;
            Created = DateTime.MinValue;
            Started = DateTime.MinValue;
            Finished = DateTime.MinValue;
            PreviousStatus = String.Empty;
            Downloaded = DateTime.MinValue;
            Uploaded = DateTime.MinValue;
            Description = String.Empty;
            Processor = String.Empty;
        }

        public override void ReadXml(XElement root)
        {
            if (root == null) return;

            // Reads <id></id>
            var elem = root.Element("id");
            MediaId = elem != null ? elem.Value : String.Empty;

            // Reads <userid></userid>
            elem = root.Element("userid");
            UserId = elem != null ? elem.Value : String.Empty;

            // Reads <sourcefile></sourcefile>
            elem = root.Element("sourcefile");
            if (elem != null)
            {
                Uri uri;
                Uri.TryCreate(elem.Value, UriKind.Absolute, out uri);
                SourceFile = uri;
            }

            // Reads <status></status>
            elem = root.Element("status");
            Status = elem != null ? elem.Value : String.Empty;

            // Reads <notifyurl></notifyurl>
            elem = root.Element("notifyurl");
            if (elem != null)
            {
                Uri uri;
                Uri.TryCreate(elem.Value, UriKind.Absolute, out uri);
                Notify = uri;
            }

            DateTime d = DateTime.MinValue;

            // Reads <created></created>
            elem = root.Element("created");
            if (elem != null)
            {
                DateTime.TryParse(elem.Value, out d);
                Created = d;
            }

            // Reads <started></started>
            elem = root.Element("started");
            if (elem != null)
            {
                DateTime.TryParse(elem.Value, out d);
                Started = d;
            }

            // Reads <finished></finished>
            elem = root.Element("finished");
            if (elem != null)
            {
                DateTime.TryParse(elem.Value, out d);
                Finished = d;
            }

            // Reads <prevstatus></prevstatus>
            elem = root.Element("prevstatus");
            PreviousStatus = elem != null ? elem.Value : String.Empty;

            // Reads <downloaded></downloaded>
            elem = root.Element("downloaded");
            if (elem != null)
            {
                DateTime.TryParse(elem.Value, out d);
                Downloaded = d;
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
        }
    }
}
