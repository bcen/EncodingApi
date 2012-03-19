using System;
using EncodingApi.Models;
using Xunit;

namespace EncodingApi.Test
{
    public class EncodingFormatTest : IDisposable
    {
        private EncodingFormat f1;

        public EncodingFormatTest ()
        {
            EncodingServiceClient client = new EncodingServiceClient();
            string xml =
@"<EncodingFormat>
<output>mp4</output>
<size>800x600</size>
<noise_reduction>2</noise_reduction>
</EncodingFormat>";
            f1 = client.Deserialize<EncodingFormat>(xml);
        }

        [Fact]
        public void TestEncodingFormatGetter()
        {
            Assert.NotNull(f1);
            Assert.Equal("mp4", f1.Output);
            Assert.Equal(2, f1.NoiseReduction);
            Assert.Equal("800x600", f1.Size);
        }

        [Fact]
        public void TestEncodingFormatSetter()
        {
            EncodingFormat f = new EncodingFormat("flv");
            f.NoiseReduction = 5;
            f.SetVideoFrameSize(800, 600);

            Assert.NotNull(f);
            Assert.Equal("flv", f.Output);
            Assert.Equal(5, f.NoiseReduction);
            Assert.Equal("800x600", f.Size);
        }

        [Fact]
        public void TestDeleteProperties()
        {
            EncodingFormat f = new EncodingFormat();

            Assert.NotNull(f);
            Assert.Equal(String.Empty, f.Output);
            Assert.Null(f.NoiseReduction);
            Assert.Null(f.GetVideoFrameSize());
        }

        public void Dispose()
        {
            f1 = null;
        }
    }
}

