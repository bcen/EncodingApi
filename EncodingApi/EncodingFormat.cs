using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using EncodingApi.Extensions;

namespace EncodingApi
{
    public class EncodingFormat : IXmlSerializable
    {
        /// <summary>
        /// Determines the level of noise filtering to apply in the preprocessor.
        /// 0 is no preprocessing, 6 is extreme preprocessing.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int NoiseReduction
        {
            get { return _noiseReduction; }
            set
            {
                if (value > 6)
                    throw new ArgumentOutOfRangeException("NoiseReduction must be less than or equal to 6.");
                _noiseReduction = value;
            }
        }
        private int _noiseReduction;

        /// <summary>
        /// Output format type.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// The video codec of the output.
        /// </summary>
        public string VideoCodec { get; set; }

        /// <summary>
        /// The audio codec of the output.
        /// </summary>
        public string AudioCodec { get; set; }

        /// <summary>
        /// The video bitrate of the output in kilo/second.
        /// Nkb/s, where N is any non-zero integer.
        /// </summary>
        public int VideoBitrate { get; set; }

        /// <summary>
        /// The audio bitrate of the output in kilo/second.
        /// Nkb/s, where N is any non-zero double.
        /// </summary>
        public double AudioBitrate { get; set; }

        /// <summary>
        /// The audio sample rate of the output in Hz.
        /// NHz, where N is any non-zero integer.
        /// </summary>
        public int AudioSampleRate { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EncodingFormat()
            : this(String.Empty)
        {
        }

        /// <summary>
        /// Initializes the EncodingFormat class with the specified output.
        /// </summary>
        /// <param name="output">The output format.</param>
        public EncodingFormat(string output)
        {
            NoiseReduction = -1;
            Output = output;
            VideoCodec = String.Empty;
            AudioCodec = String.Empty;
            VideoBitrate = 0;
            AudioBitrate = 0D;
            AudioSampleRate = 0;
        }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            XElement root = XElement.ReadFrom(reader) as XElement;
            if (root == null) return;

            var elem = root.Element("noise_reduction");
            if (elem != null)
            {
                int tmp;
                NoiseReduction = Int32.TryParse(elem.Value, out tmp) ? tmp : -1;
            }

            elem = root.Element("output");
            Output = elem != null ? elem.Value : String.Empty;

            elem = root.Element("video_codec");
            VideoCodec = elem != null ? elem.Value : String.Empty;

            elem = root.Element("audio_codec");
            AudioCodec = elem != null ? elem.Value : String.Empty;

            elem = root.Element("bitrate");
            if (elem != null)
            {
                string[] tokens = elem.Value.Split('k', 'K');
                int tmp;
                VideoBitrate = Int32.TryParse(tokens[0], out tmp) ? tmp : 0;
            }

            elem = root.Element("audio_bitrate");
            if (elem != null)
            {
                string [] tokens = elem.Value.Split('k', 'K');
                double tmp;
                AudioBitrate = Double.TryParse(tokens[0], out tmp) ? tmp : 0D;
            }

            elem = root.Element("audio_sample_rate");
            if (elem != null)
            {
                int tmp;
                AudioSampleRate = Int32.TryParse(elem.Value, out tmp) ? tmp : 0;
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (NoiseReduction >= 0)
            {
                writer.WriteSafeElementString("noise_reduction", Convert.ToString(NoiseReduction));
            }

            writer.WriteSafeElementString("output", Output);

            writer.WriteSafeElementString("video_codec", VideoCodec);

            writer.WriteSafeElementString("audio_codec", AudioCodec);

            if (VideoBitrate != 0)
            {
                writer.WriteSafeElementString("bitrate", String.Format("{0}k", VideoBitrate));
            }

            if (AudioBitrate != 0D)
            {
                writer.WriteSafeElementString("audio_bitrate", String.Format("{0}k", AudioBitrate));
            }

            if (AudioSampleRate != 0)
            {
                writer.WriteSafeElementString("audio_sample_rate", String.Format("{0}", AudioSampleRate));
            }
        }
    }
}
