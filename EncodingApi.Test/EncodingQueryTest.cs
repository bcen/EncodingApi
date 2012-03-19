using System;
using System.Linq;
using EncodingApi.Models;
using Xunit;

namespace EncodingApi.Test
{
    public class EncodingQueryTest : IDisposable
    {
        private EncodingQuery qry1;

        public EncodingQueryTest ()
        {
            EncodingServiceClient client = new EncodingServiceClient();
            string xml =
@"<?xml version=""1.0""?>
<query>
    <userid>7778</userid>
    <userkey>longkey</userkey>
    <action>AddMedia</action>
    <mediaid>1234</mediaid>
    <source>http://www.yahoo.com/mp4</source>
    <notify>http://callback.com/callback</notify>
</query>";
            qry1 = client.Deserialize<EncodingQuery>(xml);
        }

        [Fact]
        public void TestEncodingQueryGetter()
        {
            Assert.NotNull(qry1);

            Assert.Equal("7778", qry1.UserId);
            Assert.Equal("longkey", qry1.UserKey);
            Assert.Equal("AddMedia", qry1.Action);
            Assert.Equal("1234", qry1.MediaId);
            Assert.Contains(new Uri("http://www.yahoo.com/mp4"), qry1.GetAllSourceUri());
            Assert.Equal(new Uri("http://callback.com/callback"), qry1.GetNotifyUri());
            Assert.Empty(qry1.Formats);
        }

        [Fact]
        public void TestEncodingQuerySetter()
        {
            EncodingQuery qry2 = new EncodingQuery();
            qry2.UserId = qry1.UserId;
            qry2.UserKey = qry1.UserKey;
            qry2.Action = qry1.Action;
            qry2.MediaId = qry1.MediaId;
            qry2.AddSourceUri(qry1.GetAllSourceUri().First());
            qry2.SetNotifyUri(qry1.GetNotifyUri());

            Assert.NotNull(qry2);
            Assert.Equal(qry1.UserId, qry2.UserId);
            Assert.Equal(qry1.UserKey, qry2.UserKey);
            Assert.Equal(qry1.Action, qry2.Action);
            Assert.Equal(qry1.MediaId, qry2.MediaId);
            Assert.Equal(qry1.GetAllSourceUri(), qry2.GetAllSourceUri());
            Assert.Equal(qry1.GetNotifyUri(), qry2.GetNotifyUri());
            Assert.Empty(qry2.Formats);
        }

        [Fact]
        public void TestDeleteProperties()
        {
            qry1.UserId = String.Empty;
            qry1.Action = String.Empty;
            Assert.Equal(String.Empty, qry1.UserId);
        }

        public void Dispose()
        {
            qry1 = null;
        }
    }
}
