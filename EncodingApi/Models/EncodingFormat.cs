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
        /// The output format type.
        /// </summary>
        [XmlElement("output")]
        public string Output { get; set; }

        /// <summary>
        /// Video frame size. Do not call the property directly, instead use
        /// GetVideoFrameSize and SetVideoFrameSize to mutate this property.
        /// </summary>
        [XmlElement("size")]
        public string Size { get; set; }

        /// <summary>
        /// To test whether to serialize NoiseReduction or not.
        /// </summary>
        /// <returns>True if NoiseReduction is assigned with a value, otherwise false.</returns>
        public bool ShouldSerializeNoiseReduction() { return NoiseReduction.HasValue; }

        /// <summary>
        /// To test whether to serialize Output or not.
        /// </summary>
        /// <returns>True if Output is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeOutput() { return !String.IsNullOrEmpty(Output); }

        /// <summary>
        /// To test whether to serialize Size or not.
        /// </summary>
        /// <returns>True if Size is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeSize() { return !String.IsNullOrEmpty(Size); }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EncodingFormat()
            : this(String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance with the specified output type.
        /// </summary>
        /// <param name="output">The output format type.</param>
        public EncodingFormat(string output)
        {
            Output = output;
            Size = String.Empty;
        }

        /// <summary>
        /// Sets the video frame size with the specified width and height.
        /// </summary>
        /// <param name="width">The width of the new video frame size.</param>
        /// <param name="height">The hheight of the new video frame size.</param>
        public void SetVideoFrameSize(int width, int height)
        {
            SetVideoFrameSize(new VideoFrameSize(width, height));
        }

        /// <summary>
        /// Sets the video frame size with another instance of VideoFrameSize.
        /// The new width and height must be an even integer.
        /// </summary>
        /// <param name="videoFrameSize">The new VideoFrameSize.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the new width or height is not an even integer.
        /// </exception>
        public void SetVideoFrameSize(VideoFrameSize videoFrameSize)
        {
            if ((videoFrameSize.Width % 2 != 0) || (videoFrameSize.Height % 2 != 0))
                throw new ArgumentOutOfRangeException("Width and Height must be even integer.");

            Size = String.Format("{0}x{1}", videoFrameSize.Width, videoFrameSize.Height);
        }

        /// <summary>
        /// Gets the current VideoFrameSize.
        /// </summary>
        /// <returns>The current VideoFrameSize.</returns>
        public VideoFrameSize GetVideoFrameSize()
        {
            VideoFrameSize vfSize = null;
            if (!String.IsNullOrEmpty(Size))
            {
                string[] tokens = Size.Split('x', 'X');
                vfSize = new VideoFrameSize(Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]));
            }
            return vfSize;
        }
    }

    /// <summary>
    /// Representation of video frame size.
    /// </summary>
    public class VideoFrameSize
    {
        /// <summary>
        /// The width of the video frame.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the video frame.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public VideoFrameSize()
        {
        }

        /// <summary>
        /// Initializes with the specified width and height.
        /// </summary>
        /// <param name="width">The width of the video frame.</param>
        /// <param name="height">The height of the video frame.</param>
        public VideoFrameSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
