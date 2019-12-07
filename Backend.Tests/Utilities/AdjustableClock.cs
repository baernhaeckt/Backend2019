using System;
using Backend.Infrastructure.Abstraction.Hosting;

namespace Backend.Tests.Utilities
{
    public class AdjustableClock : IClock
    {
        public Func<DateTimeOffset> GetNow { get; set; } = () => DateTimeOffset.UtcNow;

        public DateTimeOffset Now() => GetNow();
    }
}