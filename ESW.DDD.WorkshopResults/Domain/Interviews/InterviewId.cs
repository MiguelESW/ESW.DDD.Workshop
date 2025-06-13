namespace ESW.DDD.WorkshopResults.Domain.Interviews;

internal record InterviewId
{
    public static InterviewId New()
    {
        return new(Guid.NewGuid());
    }

    public Guid Value { get; }

    public override string ToString() => Value.ToString();

    private InterviewId(Guid value) => Value = value;
}
