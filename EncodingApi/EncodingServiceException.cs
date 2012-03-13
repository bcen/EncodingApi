using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncodingApi
{
    public class EncodingServiceException : Exception
    {
        public EncodingServiceException(string message)
            : base(message)
        {
        }
    }
}
