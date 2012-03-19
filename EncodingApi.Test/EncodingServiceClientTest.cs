using System;
using Xunit;
using EncodingApi;

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
            Assert.Throws<EncodingServiceException>(() =>
            {
                client.GetMediaList();
            });
        }

        [Fact]
        public void TestSendGetMediaList()
        {
            var response = client.SendGetMediaListRequest();
            Assert.NotEmpty(response.Errors);
        }

        [Fact]
        public void TestNoAuth()
        {
            client.UserId = null;
            client.UserKey = null;
            Assert.Throws<EncodingServiceException>(() =>
            {
                client.SendGetMediaListRequest();
            });
        }

        public void Dispose()
        {
            client = null;
        }
    }
}