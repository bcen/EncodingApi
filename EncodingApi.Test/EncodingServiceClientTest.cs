using System;
using Xunit;
using EncodingApi.Models;

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

            Assert.Throws<EncodingServiceException>(() =>
            {
                client.AddMedia(new Uri[]{ }, new EncodingFormat[]{ }, 
                                notifyUri: new Uri("http://www.yahoo.com"));
            });
        }

        public void Dispose()
        {
            client = null;
        }
    }
}
