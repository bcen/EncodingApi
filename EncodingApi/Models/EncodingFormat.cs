using System;
using System.Xml.Serialization;

namespace EncodingApi.Models
{
    public class EncodingFormat
    {
        /// <summary>
        /// Determines the level of noise filtering to apply in the preprocessor. 
        /// 0 is no preprocessing, 6 is extreme preprocessing.
        /// </summary>
        [XmlElement("noise_reduction")]
        public int? NoiseReduction { get; set; }

        /// <summary>
        /// Format type.
        /// </summary>
        [XmlElement("output")]
        public string Output { get; set; }

        /// <summary>
        /// Video frame size. Do not call the property directly, instead use
        /// GetVideoFrameSize and SetVideoFrameSize to change this property.
        /// </summary>
        [XmlElement("size")]
        public string Size { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EncodingFormat()
        {
        }

        /// <summary>
        /// Create a new instance with the specified output type.
        /// </summary>
        /// <param name="output">The output format type.</param>
        public EncodingFormat(string output)
        {
            Output = output;
        }

        /// <summary>
        /// To test should NoiseReduction be serialized.
        /// </summary>
        /// <returns>Ture if NoiseReduction has value, else false.</returns>
        public bool ShouldSerializeNoiseReduction()
        {
            return NoiseReduction.HasValue;
        }

        public void SetVideoFrameSize(int width, int height)
        {
            SetVideoFrameSize(new VideoFrameSize(width, height));
        }

        public void SetVideoFrameSize(VideoFrameSize videoFrameSize)
        {
            if ((videoFrameSize.Width % 2 != 0) || (videoFrameSize.Height % 2 != 0))
                throw new ArgumentOutOfRangeException("Width and Height must be even integer.");

            Size = String.Format("{0}x{1}", videoFrameSize.Width, videoFrameSize.Height);
        }

        public VideoFrameSize GetVideoFrameSize()
        {
            VideoFrameSize vfSize = null;
            if (Size != null)
            {
                string[] tokens = Size.Split('x', 'X');
                vfSize = new VideoFrameSize(Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]));
            }
            return vfSize;
        }
    }

    public class VideoFrameSize
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public VideoFrameSize()
        {
        }

        public VideoFrameSize(int width, int height)
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
