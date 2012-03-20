using System;
using System.Collections.Generic;
using EncodingApi.Models;

namespace EncodingApi
{
    public partial class EncodingServiceClient
    {
        public virtual void GetResponseAsync<T>(EncodingQuery query, Action<T> callback)
            where T : class, new()
        {
            if (UserId == null || UserKey == null)
                throw new EncodingServiceException("UserId or UserKey is empty");

            query.UserId = UserId;
            query.UserKey = UserKey;

            GetXmlResponseAsync(Serialize(query), (xml) =>
            {
                callback(Deserialize<T>(xml));
            });
        }

        public void GetMediaListAsync(Action<ICollection<GetMediaListResponse.Media>> callback,
                                      Action<ICollection<string>> errors)
        {
            GetResponseAsync<GetMediaListResponse>(EncodingQuery.CreateGetMediaListQuery(),
            (response) =>
            {
                callback(response.MediaList);
                errors(response.Errors);
            });
        }
    }
}
