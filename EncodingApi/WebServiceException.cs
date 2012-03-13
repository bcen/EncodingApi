using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncodingApi
{
    public class WebServiceException : Exception
    {
        public WebServiceException(string message)
            : base(message)
        {
        }
    }
}
