using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Framework.Web
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class NotEmptyAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be empty";

        public NotEmptyAttribute()
            : base(DefaultErrorMessage)
        {
        }

        public override bool IsValid(object value) => value is null || value switch
        {
            Guid guid => guid != Guid.Empty,
            _ => true
        };
    }
}