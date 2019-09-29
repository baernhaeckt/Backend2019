using System;

namespace Backend.Core.Features.UserManagement.Security
{
    public class CannotPerformOperationException : Exception
    {
        public CannotPerformOperationException()
        {
        }

        public CannotPerformOperationException(string message)
            : base(message)
        {
        }

        public CannotPerformOperationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}