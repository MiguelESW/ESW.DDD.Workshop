using ESW.DDD.WorkshopResults.Domain.Common.Abstractions;

namespace ESW.DDD.WorkshopResults.Domain.Common;

internal record DateRange : IValueObject
{
    public static DateRange Create(DateTimeOffset start, DateTimeOffset end)
    {
        if (start > end)
        {
            throw new ArgumentException("Start date must be earlier than or equal to end date.", nameof(start));
        }

        return new(start, end);
    }

    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }

    public bool HaveStarted => Start < DateTimeOffset.UtcNow;

    private DateRange(DateTimeOffset start, DateTimeOffset end) 
        => (Start, End) = (start, end);
}
