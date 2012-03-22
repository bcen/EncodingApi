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
        /// The video codec of the output.
        /// </summary>
        [XmlElement("video_codec")]
        public string VideoCodec { get; set; }

        /// <summary>
        /// The audio codec of the output. If you specify AudioCodec equal to 'copy',
        /// the options AudioChannelsNumber, AudioSampleRate, AudioBitrate will be
        /// ignored and their values will be copied from your source file.
        /// </summary>
        [XmlElement("audio_codec")]
        public string AudioCodec { get; set; }

        /// <summary>
        /// The video bitrate of the output.
        /// </summary>
        [XmlElement("bitrate")]
        public string Bitrate { get; set; }

        /// <summary>
        /// The audio bitrate of the output.
        /// </summary>
        [XmlElement("audio_bitrate")]
        public string AudioBitrate { get; set; }

        /// <summary>
        /// The audio sample rate of the output.
        /// </summary>
        [XmlElement("audio_sample_rate")]
        public int? AudioSampleRate { get; set; }

        /// <summary>
        /// The audio volume of the output in percentage.
        /// </summary>
        [XmlElement("audio_volume")]
        public int? AudioVolume { get; set; }

        /// <summary>
        /// Video frame size. Do not call the property directly, instead use
        /// GetVideoFrameSize and SetVideoFrameSize to mutate this property.
        /// </summary>
        [XmlElement("size")]
        public string Size { get; set; }

        /// <summary>
        /// The fade in effect parameter of the output.
        /// Add fade in effect to audio and video streams.
        /// </summary>
        [XmlElement("fade_in")]
        public string FadeIn { get; set; }

        /// <summary>
        /// The fade out effect parameter of the output.
        /// Add fade out effect to audio and video streams.
        /// </summary>
        [XmlElement("fade_out")]
        public string FadeOut { get; set; }

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
        /// To test whether to serialize VideoCodec or not.
        /// </summary>
        /// <returns>True if VideoCodec is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeVideoCodec() { return !String.IsNullOrEmpty(VideoCodec); }

        /// <summary>
        /// To test whether to serialize AudioCodec or not.
        /// </summary>
        /// <returns>True if AudioCodec is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeAudioCodec() { return !String.IsNullOrEmpty(AudioCodec); }

        /// <summary>
        /// To test whether to serialize Bitrate or not.
        /// </summary>
        /// <returns>True if Bitrate is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeBitrate() { return !String.IsNullOrEmpty(Bitrate); }

        /// <summary>
        /// To test whether to serialize AudioBitrate or not.
        /// </summary>
        /// <returns>True if AudioBitrate is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeAudioBitrate() { return !String.IsNullOrEmpty(AudioBitrate); }

        /// <summary>
        /// To test whether to serialize AudioSampleRate or not.
        /// </summary>
        /// <returns>True if AudioSampleRate is assigned with a value, otherwise false.</returns>
        public bool ShouldSerializeAudioSampleRate() { return AudioSampleRate.HasValue; }

        /// <summary>
        /// To test whether to serialize AudioVolume or not.
        /// </summary>
        /// <returns>True if AudioVolume is assigned with a value, otherwise false.</returns>
        public bool ShouldSerializeAudioVolume() { return AudioVolume.HasValue; }

        /// <summary>
        /// To test whether to serialize Size or not.
        /// </summary>
        /// <returns>True if Size is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeSize() { return !String.IsNullOrEmpty(Size); }

        /// <summary>
        /// To test whether to serialize FadeIn or not.
        /// </summary>
        /// <returns>True if FadeIn is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeFadeIn() { return !String.IsNullOrEmpty(FadeIn); }

        /// <summary>
        /// To test whether to serialize FadeOut or not.
        /// </summary>
        /// <returns>True if FadeOut is not null nor empty string, otherwise false.</returns>
        public bool ShouldSerializeFadeOut() { return !String.IsNullOrEmpty(FadeOut); }

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
            VideoCodec = String.Empty;
            AudioCodec = String.Empty;
            Bitrate = String.Empty;
            AudioBitrate = String.Empty;
            FadeIn = String.Empty;
            FadeOut = String.Empty;
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

        /// <summary>
        /// Sets the video bitrate of the output.
        /// </summary>
        /// <param name="bitrate">The bitrate of the output.</param>
        public void SetVideoBitrate(int bitrate)
        {
            if (bitrate == 0)
                throw new ArgumentException("bitrate must be non-zero integer.");

            Bitrate = String.Format("{0}k", bitrate);
        }

        /// <summary>
        /// Gets the video bitrate of the output.
        /// </summary>
        /// <returns>The bitrate of the output.</returns>
        public int GetVideoBitrate()
        {
            string[] tokens = Bitrate.Split('k', 'K');
            int bitrate = Convert.ToInt32(tokens[0]);
            return bitrate;
        }

        /// <summary>
        /// Sets the audio bitrate of the output.
        /// </summary>
        /// <param name="bitrate">The new bitrate of the output.</param>
        public void SetAudioBitrate(double bitrate)
        {
            if (bitrate == 0D)
                throw new ArgumentException("bitrate cannot be zero.");

            AudioBitrate = String.Format("{0}k", bitrate);
        }

        /// <summary>
        /// Gets the audio bitrate of the output.
        /// </summary>
        /// <returns>The bitrate of the output.</returns>
        public double GetAudioBitrate()
        {
            string[] tokens = AudioBitrate.Split('k', 'K');
            double bitrate = Convert.ToDouble(tokens[0]);
            return bitrate;
        }

        /// <summary>
        /// Sets the fade in time parameters.
        /// </summary>
        /// <param name="param">The new effect time parameters.</param>
        public void SetFadeInTime(EffectTimeParameters param)
        {
            if (param.Start < 0D || param.Duration < 0D)
                throw new ArgumentException("Start time and duration cannot be less than 0.");

            FadeIn = String.Format("{0}:{1}", param.Start, param.Duration);
        }

        /// <summary>
        /// Sets the fade in time parameters.
        /// </summary>
        /// <param name="start">The fade in start time in seconds.</param>
        /// <param name="duration">The fade in duration in seconds.</param>
        public void SetFadeInTime(double start, double duration)
        {
            SetFadeInTime(new EffectTimeParameters(start, duration));
        }

        /// <summary>
        /// Gets the fade in time parameters.
        /// </summary>
        /// <returns>The fade in time parameters of the output.</returns>
        public EffectTimeParameters GetFadeInTime()
        {
            string[] tokens = FadeIn.Split(':');
            return new EffectTimeParameters(Convert.ToDouble(tokens[0]),
                                            Convert.ToDouble(tokens[1]));
        }

        /// <summary>
        /// Sets the fade out time parameters.
        /// </summary>
        /// <param name="param">The new effect time parameters.</param>
        public void SetFadeOutTime(EffectTimeParameters param)
        {
            if (param.Start < 0D || param.Duration < 0D)
                throw new ArgumentException("Start time and duration cannot be less than 0.");

            FadeOut = String.Format("{0}:{1}", param.Start, param.Duration);
        }

        /// <summary>
        /// Sets the fade out time parameters.
        /// </summary>
        /// <param name="start">The fade out start time in seconds.</param>
        /// <param name="duration">The fade out duration in seconds.</param>
        public void SetFadeOutTime(double start, double duration)
        {
            SetFadeOutTime(new EffectTimeParameters(start, duration));
        }

        /// <summary>
        /// Gets the fade out time parameters.
        /// </summary>
        /// <returns>The fade in time parameters of the output.</returns>
        public EffectTimeParameters GetFadeOutTime()
        {
            string[] tokens = FadeOut.Split(':');
            return new EffectTimeParameters(Convert.ToDouble(tokens[0]),
                                            Convert.ToDouble(tokens[1]));
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

    public class EffectTimeParameters
    {
        /// <summary>
        /// The fade in/out start time in seconds.
        /// </summary>
        public double Start { get; set; }

        /// <summary>
        /// The fade in/out duration in seconds.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EffectTimeParameters()
        {
        }

        /// <summary>
        /// Initializes the FadeInEffect class with the specified start time and duration.
        /// </summary>
        /// <param name="start">The start time in seconds of the fade in/out effect.</param>
        /// <param name="duration">The duration in seconds of the fade in/out effect.</param>
        public EffectTimeParameters(double start, double duration)
        {
            Start = start;
            Duration = duration;
        }
    }
}
