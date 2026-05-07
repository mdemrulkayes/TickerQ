using System;
using TickerQ.Utilities.Enums;

namespace TickerQ.Dashboard.Hubs
{
    internal sealed class CronOccurrenceUpdateNotification
    {
        public Guid Id { get; set; }
        public TickerStatus Status { get; set; }
        public Guid? CronTickerId { get; set; }
        public DateTime ExecutedAt { get; set; }
        public long ElapsedTime { get; set; }
        public int RetryCount { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
