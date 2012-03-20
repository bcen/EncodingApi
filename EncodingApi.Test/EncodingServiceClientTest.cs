using System;
using Xunit;

namespace EncodingApi.Test
{
    public class EncodingServiceClientTest : IDisposable
    {
        private EncodingServiceClient client;

        public EncodingServiceClientTest()
        {
            client = new EncodingServiceClient("id", "key");
        }

        [Fact]
        public void TestGetMediaListException()
        {
            client.UserId = "invalid_id";
            client.UserKey = "invalid_key";

            Assert.Throws<EncodingServiceException>(() =>
            {
                client.GetMediaList();
            });
        }

        public void Dispose()
        {
            client = null;
        }
    }
}
