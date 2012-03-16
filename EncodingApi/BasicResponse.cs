using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EncodingApi
{
    /// <summary>
    /// Base class for all response class.
    /// </summary>
    [XmlRoot("response")]
    public abstract class BasicResponse
    {
        /// <summary>
        /// Message from response.
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; }

        /// <summary>
        /// List of error.
        /// </summary>
        [XmlArray("errors")]
        [XmlArrayItem("error")]
        public List<string> Errors { get; set; }

        /// <summary>
        /// To test whether to serialize Message or not.
        /// </summary>
        /// <returns>True if Message is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeMessage() { return !String.IsNullOrEmpty(Message); }

        /// <summary>
        /// To test wether to serialize Errors or not.
        /// </summary>
        /// <returns>True if Errors count is greater than zero, otherwise false.</returns>
        public bool ShouldSerializeErrors() { return (Errors.Count > 0); }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BasicResponse()
        {
            Message = string.Empty;
            Errors = new List<string>();
        }
    }
}
