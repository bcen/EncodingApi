using System;

namespace EncodingApi
{
    public class EncodingFormat : Internal.XmlModelBase
    {
        public EncodingFormat()
            : this("<format/>")
        {
        }

        public EncodingFormat(string xml)
            : base(xml)
        {
        }

        public int NoiseReduction
        {
            get
            {
                string text = GetXmlElementInnerText("noise_reduction");
                return String.IsNullOrEmpty(text) ? -1 : Convert.ToInt32(text);
            }
            set
            {
                string text = value < 0 ? null : Convert.ToString(value);
                SetXmlElementInnerText("noise_reduction", text);
            }
        }

        public string Output
        {
            get
            {
                return GetXmlElementInnerText("output");
            }
            set
            {
                SetXmlElementInnerText("output", value);
            }
        }

        public string VideoCodec
        {
            get
            {
                return GetXmlElementInnerText("video_codec");
            }
            set
            {
                SetXmlElementInnerText("video_codec", value);
            }
        }

        public string AudioCodec
        {
            get
            {
                return GetXmlElementInnerText("audio_codec");
            }
            set
            {
                SetXmlElementInnerText("audio_codec", value);
            }
        }

        public string[] Bitrate
        {
            get
            {
                string text = GetXmlElementInnerText("bitrate");
                return text == null ? null : text.Split(',');
            }
            set
            {
                string text = (value == null ? null : String.Join(",", value));
                SetXmlElementInnerText("bitrate", text);
            }
        }

        public double AudioBitrate
        {
            get
            {
                double ab = 0.0;
                string text = GetXmlElementInnerText("audio_bitrate");
                string[] tokens = String.IsNullOrEmpty(text) ? null : text.Split('k', 'K');
                if (tokens != null && tokens.Length >= 1)
                    ab = Convert.ToDouble(tokens[0]);
                return ab;
            }
            set
            {
                string text = null;
                if (value > 0)
                    text = String.Format("{0}k", Convert.ToString(value));
                SetXmlElementInnerText("audio_bitrate", text);
            }
        }

        public int AudioSampleRate
        {
            get
            {
                string text = GetXmlElementInnerText("audio_sample_rate");
                return String.IsNullOrEmpty(text) ? 0 : Convert.ToInt32(text);
            }
            set
            {
                string text = value <= 0 ? null : Convert.ToString(value);
                SetXmlElementInnerText("audio_sample_rate", text);
            }
        }

        public int AudioVolume
        {
            get
            {
                string text = GetXmlElementInnerText("audio_volume");
                return String.IsNullOrEmpty(text) ? -1 : Convert.ToInt32(text);
            }
            set
            {
                string text = (value < 0 || value > 100) ? null : Convert.ToString(value);
                SetXmlElementInnerText("audio_volume", text);
            }
        }

        public VideoSize Size
        {
            get
            {
                string text = GetXmlElementInnerText("size");
                string[] tokens = String.IsNullOrEmpty(text) ? null : text.Split('x', 'X');
                VideoSize vSize = null;

                if (tokens != null && tokens.Length >= 2)
                {
                    vSize = new VideoSize(Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]));
                }

                return vSize;
            }
            set
            {
                string text = null;
                if (value != null)
                {
                    text = String.Format("{0}x{1}", value.Width, value.Height);
                }

                SetXmlElementInnerText("size", text);
            }
        }

        public FadeTime FadeIn
        {
            get
            {
                string text = GetXmlElementInnerText("fade_in");
                string[] tokens = String.IsNullOrEmpty(text) ? null : text.Split(':');
                FadeTime fTime = null;

                if (tokens != null && tokens.Length >= 2)
                {
                    fTime = new FadeTime(Convert.ToDouble(tokens[0]), 
                                         Convert.ToDouble(tokens[1]));
                }

                return fTime;
            }
            set
            {
                string text = null;
                if (value != null)
                {
                    text = String.Format("{0}:{1}", value.Start, value.Duration);
                }

                SetXmlElementInnerText("fade_in", text);
            }
        }

        public FadeTime FadeOut
        {
            get
            {
                string text = GetXmlElementInnerText("fade_out");
                string[] tokens = String.IsNullOrEmpty(text) ? null : text.Split(':');
                FadeTime fTime = null;

                if (tokens != null && tokens.Length >= 2)
                {
                    fTime = new FadeTime(Convert.ToDouble(tokens[0]),
                                         Convert.ToDouble(tokens[1]));
                }

                return fTime;
            }
            set
            {
                string text = null;
                if (value != null)
                {
                    text = String.Format("{0}:{1}", value.Start, value.Duration);
                }

                SetXmlElementInnerText("fade_out", text);
            }
        }

        public int CropLeft
        {
            get
            {
                string text = GetXmlElementInnerText("crop_left");
                return String.IsNullOrEmpty(text) ? -1 : Convert.ToInt32(text);
            }
            set
            {
                string text = null;
                if (value % 2 == 0)
                    text = Convert.ToString(value);
                SetXmlElementInnerText("crop_left", text);
            }
        }
    }

    public class VideoSize
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public VideoSize()
        {
        }

        public VideoSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    public class FadeTime
    {
        public double Start { get; set; }
        public double Duration { get; set; }

        public FadeTime()
        {
        }

        public FadeTime(double start, double duration)
        {
            Start = start;
            Duration = duration;
        }
    }
}
