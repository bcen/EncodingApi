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
        public void TestNullIdAndKey()
        {
            client.UserId = null;
            client.UserKey = null;

            try
            {
                client.GetResponse<GetMediaListResponse>(new Models.EncodingQuery());
            }
            catch (EncodingServiceException ex)
            {
                Assert.Equal("UserId or UserKey is empty", ex.Message);
            }
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
