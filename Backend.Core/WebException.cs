using System;
using System.Net;

namespace Backend.Core
{
    public class WebException : Exception
    {
        public WebException()
        {
        }

        public WebException(string message)
            : base(message)
        {
        }

        public WebException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public WebException(string message, HttpStatusCode httpStatusCode)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode HttpStatusCode { get; }
    }
}