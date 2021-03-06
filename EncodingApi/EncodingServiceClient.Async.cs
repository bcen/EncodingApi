﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EncodingApi
{
    public partial class EncodingServiceClient
    {
        /// <summary>
        /// Asynchronous version of <see cref="GetResponse"/>.
        /// </summary>
        /// <param name="callback">Callback action for T.</param>
        public virtual void GetResponseAsync<T>(EncodingQuery query, Action<T> callback)
            where T : BasicResponse, new()
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

        /// <summary>
        /// Asynchronous version of <see cref="GetMediaList"/>.
        /// </summary>
        /// <param name="callback">Callback action for <c>MediaList</c>.</param>
        /// <param name="errors">Callback action for <c>Errors</c>.</param>
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

        /// <summary>
        /// Asynchronous version of <see cref="AddMedia"/>.
        /// </summary>
        /// <param name="callback">Callback action for the <c>MediaId</c>.</param>
        /// <param name="errors">Callback action for <c>Errors</c>.</param>
        public void AddMediaAsync(IEnumerable<Uri> sources, IEnumerable<EncodingFormat> formats,
                                  Action<string> callback, Action<ICollection<string>> errors,
                                  bool isInstant=false, Uri notifyUri=null)
        {
            if (sources == null || formats == null)
                throw new ArgumentNullException("sources or formats cannot be null.");

            EncodingQuery query = new EncodingQuery();
            query.Action = EncodingQuery.QueryAction.AddMedia;
            query.Formats = formats.ToList();
            query.Notify = notifyUri;
            foreach (Uri uri in sources)
            {
                query.Sources.Add(uri);
            }
            query.IsInstant = isInstant;

            GetResponseAsync<AddMediaResponse>(query, (response) =>
            {
                callback(response.MediaId);
                errors(response.Errors);
            });
        }

        /// <summary>
        /// Asynchronous version of <see cref="RestartMediaErrors"/>.
        /// </summary>
        /// <param name="callback">Callback action for <c>Message</c>.</param>
        /// <param name="errors">Callback action for <c>Errors</c>.</param>
        public void RestartMediaErrorsAsync(string mediaId, Action<string> callback,
                                            Action<ICollection<string>> errors)
        {
            if (String.IsNullOrEmpty(mediaId))
                throw new ArgumentNullException("mediaId cannot be null nor empty string.");

            var q = EncodingQuery.CreateRestartMediaErrorsQuery(mediaId);
            GetResponseAsync<BasicResponse>(q,
            (response) =>
            {
                callback(response.Message);
                errors(response.Errors);
            });
        }
    }
}
