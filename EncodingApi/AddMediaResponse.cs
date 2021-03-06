﻿using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using EncodingApi.Extensions;

namespace EncodingApi
{
    /// <summary>
    /// Encapsulates AddMedia XML response.
    /// </summary>
    [XmlRoot("response")]
    public sealed class AddMediaResponse : BasicResponse
    {
        /// <summary>
        /// The ID of the added media.
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddMediaResponse()
            : base()
        {
            MediaId = String.Empty;
        }

        /// <summary>
        /// Reads XML from <c>root</c> into this object instance.
        /// </summary>
        /// <param name="root">The XElement to read from.</param>
        public override void ReadXml(XElement root)
        {
            if (root == null) return;

            base.ReadXml(root);

            // Reads <mediaid></mediaid>
            var elem = root.Element("mediaid");
            MediaId = elem != null ? elem.Value : String.Empty;
        }

        /// <summary>
        /// Writes this object into XML representation.
        /// </summary>
        /// <param name='writer'>The XmlWriter to write to.</param>
        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            // Writes <mediaid>MediaId</mediaid>
            writer.WriteSafeElementString("mediaid", MediaId);
        }
    }
}
