using System;

namespace Backend.Core.Features.Partner.Data.Testing
{
    public static class TokenIssuerTestCredentials
    {
        public const string Secret1 = "1234";

        public const string Secret2 = "1234";

        public const string Secret3 = "1234";

        public static Guid Id1 { get; } = Guid.Parse("ccc14b11-5922-4e3e-bb54-03e71facaeb3");

        public static Guid Id2 { get; } = Guid.Parse("bcc14b11-5922-4e3e-bb54-03e71facaeb3");

        public static Guid Id3 { get; } = Guid.Parse("acc14b11-5922-4e3e-bb54-03e71facaeb3");
    }
}