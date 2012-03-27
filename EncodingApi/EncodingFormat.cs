using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using EncodingApi.Extensions;

namespace EncodingApi
{
    [XmlRoot("format")]
    public class EncodingFormat : XmlSerializableObject
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
        /// Audio volume of the output. Any non-negative value, negative integer indicates
        /// audio volume will be default from input source.
        /// </summary>
        public int AudioVolume { get; set; }

        /// <summary>
        /// The video frame size of the output. All dimensions must be even integer.
        /// </summary>
        public Dimension VideoFrameSize { get; set; }

        /// <summary>
        /// The specified fade in time parameters of the output.
        /// </summary>
        public FadingEffectTimeParameters FadeIn { get; set; }

        public FadingEffectTimeParameters FadeOut { get; set; }

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
            AudioVolume = -1;
            VideoFrameSize = null;
            FadeIn = null;
            FadeOut = null;
        }

        public override void ReadXml(XElement root)
        {
            if (root == null) return;

            // Reads <noise_reduction></noise_reduction>
            var elem = root.Element("noise_reduction");
            if (elem != null)
            {
                int tmp;
                NoiseReduction = Int32.TryParse(elem.Value, out tmp) ? tmp : -1;
            }

            // Reads <output></output>
            elem = root.Element("output");
            Output = elem != null ? elem.Value : String.Empty;

            // Reads <video_codec></video_codec>
            elem = root.Element("video_codec");
            VideoCodec = elem != null ? elem.Value : String.Empty;

            // Reads <audio_codec></audio_codec>
            elem = root.Element("audio_codec");
            AudioCodec = elem != null ? elem.Value : String.Empty;

            // Reads <bitrate></bitrate>
            elem = root.Element("bitrate");
            if (elem != null)
            {
                string[] tokens = elem.Value.Split('k', 'K');
                int tmp;
                VideoBitrate = Int32.TryParse(tokens[0], out tmp) ? tmp : 0;
            }

            // Reads <audio_bitrate></audio_bitrate>
            elem = root.Element("audio_bitrate");
            if (elem != null)
            {
                string [] tokens = elem.Value.Split('k', 'K');
                double tmp;
                AudioBitrate = Double.TryParse(tokens[0], out tmp) ? tmp : 0D;
            }

            // Reads <audio_sample_rate></audio_sample_rate>
            elem = root.Element("audio_sample_rate");
            if (elem != null)
            {
                int tmp;
                AudioSampleRate = Int32.TryParse(elem.Value, out tmp) ? tmp : 0;
            }

            // Reads <audio_volume></audio_volume>
            elem = root.Element("audio_volume");
            if (elem != null)
            {
                int tmp;
                AudioVolume = Int32.TryParse(elem.Value, out tmp) ? tmp : 0;
            }

            // Reads <size></size>
            elem = root.Element("size");
            if (elem != null)
            {
                string[] tokens = elem.Value.Split('x', 'X');
                int w, h;
                if (Int32.TryParse(tokens[0], out w) && Int32.TryParse(tokens[1], out h))
                {
                    VideoFrameSize = new Dimension(w, h);
                }
            }

            // Reads <fade_in></fade_in>
            elem = root.Element("fade_in");
            if (elem != null)
            {
                string[] tokens = elem.Value.Split(':');
                double s, d;
                if (Double.TryParse(tokens[0], out s) && Double.TryParse(tokens[1], out d))
                {
                    FadeIn = new FadingEffectTimeParameters(s, d);
                }
            }

            // Reads <fade_out></fade_out>
            elem = root.Element("fade_out");
            if (elem != null)
            {
                string[] tokens = elem.Value.Split(':');
                double s, d;
                if (Double.TryParse(tokens[0], out s) && Double.TryParse(tokens[1], out d))
                {
                    FadeOut = new FadingEffectTimeParameters(s, d);
                }
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            // Writes <noise_reduction></noise_reduction>
            if (NoiseReduction >= 0)
            {
                writer.WriteSafeElementString("noise_reduction", Convert.ToString(NoiseReduction));
            }

            // Writes <output></output>
            writer.WriteSafeElementString("output", Output);

            // Writes <video_codec></video_codec>
            writer.WriteSafeElementString("video_codec", VideoCodec);

            // Writes <audio_codec></audio_codec>
            writer.WriteSafeElementString("audio_codec", AudioCodec);

            // Writes <bitrate></bitrate>
            if (VideoBitrate != 0)
            {
                writer.WriteSafeElementString("bitrate", String.Format("{0}k", VideoBitrate));
            }

            // Writes <audio_bitrate></audio_bitrate>
            if (AudioBitrate != 0D)
            {
                writer.WriteSafeElementString("audio_bitrate", String.Format("{0}k", AudioBitrate));
            }

            // Writes <audio_sample_rate></audio_sample_rate>
            if (AudioSampleRate != 0)
            {
                writer.WriteSafeElementString("audio_sample_rate", 
                                              String.Format("{0}", AudioSampleRate));
            }

            // Writes <audio_volume></audio_volume>
            if (AudioVolume >= 0 && AudioVolume <= 100)
            {
                writer.WriteSafeElementString("audio_volume", String.Format("{0}", AudioVolume));
            }

            // Writes <size></size>
            if (VideoFrameSize != null)
            {
                writer.WriteSafeElementString("size", 
                    String.Format("{0}x{1}", VideoFrameSize.Width, VideoFrameSize.Height));
            }

            // Writes <fade_in></fade_in>
            if (FadeIn != null)
            {
                writer.WriteSafeElementString("fade_in", 
                    String.Format("{0}:{1}", FadeIn.StartTime, FadeIn.Duration));
            }

            // Writes <fade_out></fade_out>
            if (FadeOut != null)
            {
                writer.WriteSafeElementString("fade_out",
                    String.Format("{0}:{1}", FadeOut.StartTime, FadeOut.Duration));
            }
        }
    }

    public class Dimension
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Dimension()
        {
        }

        public Dimension(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    public class FadingEffectTimeParameters
    {
        public double StartTime { get; set; }
        public double Duration { get; set; }

        public FadingEffectTimeParameters()
        {
        }

        public FadingEffectTimeParameters(double startTime, double duration)
        {
            StartTime = startTime;
            Duration = duration;
        }
    }
}
