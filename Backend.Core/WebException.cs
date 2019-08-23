using System;
using System.Net;

namespace Backend.Core
{
    public class WebException : Exception
    {
        public WebException(string message, HttpStatusCode httpStatusCode)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode HttpStatusCode { get; }
    }
}
