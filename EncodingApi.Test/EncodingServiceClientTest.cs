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
        public void SyncClientShouldThrowExceptionWithInvalidCredential()
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

        [Fact]
        public void AsyncClientShouldNotThrowExceptionWithInvalidCredential()
        {
            client.UserId = "invalid_id";
            client.UserKey = "invalid_key";

            Assert.DoesNotThrow(() =>
            {
                client.GetMediaListAsync(
                (mediaList) =>
                {
                },
                (errors) =>
                {
                });
            });

            Uri[] sources = new Uri[]{};
            EncodingFormat[] formats = new EncodingFormat[]{};

            Assert.DoesNotThrow(() =>
            {
                client.AddMediaAsync(sources, formats,
                (mediaId) =>
                {
                },
                (errors) =>
                {
                });
            });
        }

        public void Dispose()
        {
            client = null;
        }
    }
}
