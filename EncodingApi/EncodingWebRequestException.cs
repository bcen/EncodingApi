using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncodingApi
{
    public class EncodingWebRequestException : Exception
    {
        public EncodingWebRequestException(string message)
            : base(message)
        {
        }
    }
}
