using System;
using NServiceBus;

namespace NServiceBusMail
{
    public static class NServicebusExtensions
    {
        public static DateTime TimeSent(this IBus bus)
        {
            return DateTimeExtensions.ToUtcDateTime(bus.CurrentMessageContext.Headers[Headers.TimeSent]);
        }
    }
}