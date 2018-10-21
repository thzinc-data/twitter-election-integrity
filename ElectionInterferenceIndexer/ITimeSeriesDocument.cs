using System;

namespace ElectionInterferenceIndexer
{
    public interface ITimeSeriesDocument<TId>
    {
        TId Id { get; }
        DateTimeOffset Timestamp { get; }
    }
}
