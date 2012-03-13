using System;

namespace EncodingApi
{
    public class EncodingServiceException : Exception
    {
        public EncodingServiceException(string message)
            : base(message)
        {
        }

        public EncodingServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
