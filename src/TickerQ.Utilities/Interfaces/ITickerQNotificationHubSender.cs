using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Models;

namespace TickerQ.Utilities.Interfaces
{
    internal interface ITickerQNotificationHubSender
    {
        Task AddCronTickerNotifyAsync(object cronTicker);
        Task UpdateCronTickerNotifyAsync(object cronTicker);
        Task RemoveCronTickerNotifyAsync(Guid id);
        Task AddTimeTickerNotifyAsync(Guid id);
        Task AddTimeTickersBatchNotifyAsync();
        Task UpdateTimeTickerNotifyAsync(object timeTicker);
        Task RemoveTimeTickerNotifyAsync(Guid id);
        void UpdateActiveThreads(string activeThreads);
        void UpdateNextOccurrence(DateTime? nextOccurrence);
        void UpdateHostStatus(bool active);
        void UpdateHostException(string exceptionMessage);
        Task UpdateNodeHeartBeatAsync(JsonElement nodeHeartBeat);
        Task AddCronOccurrenceAsync(Guid groupId, object occurrence);
        Task UpdateCronOccurrenceAsync(Guid groupId, object occurrence);
        Task UpdateTimeTickerFromInternalFunctionContext<TTimeTickerEntity>(InternalFunctionContext internalFunctionContext) where TTimeTickerEntity : TimeTickerEntity<TTimeTickerEntity>, new();
        Task UpdateCronOccurrenceFromInternalFunctionContext<TCronTickerEntity>(InternalFunctionContext internalFunctionContext) where TCronTickerEntity : CronTickerEntity, new();
        Task CanceledTickerNotifyAsync(Guid id);
    }
}
