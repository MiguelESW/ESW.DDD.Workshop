using ESW.DDD.WorkshopResults.Domain.Common.Abstractions;
using ESW.DDD.WorkshopResults.Domain.Users;

namespace ESW.DDD.WorkshopResults.Domain.Interviews;

internal record Evaluation : IValueObject
{
    public static Evaluation Create(
        UserId interviewerId,
        int score,
        string comments)
    {
        if (score < 0 || score > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(score),
                "Score must be between 0 and 10.");
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(comments);

        return new(
            interviewerId: interviewerId,
            score: score,
            comments: comments,
            date: DateTimeOffset.UtcNow);
    }

    public UserId InterviewerId { get; }
    public int Score { get; }
    public string Comments { get; }
    public DateTimeOffset Date { get; }

    public override string ToString()
    {
        return $"Evaluation by {InterviewerId.Id} on {Date:yyyy-MM-dd}: Score={Score}, Comments='{Comments}'";
    }

    private Evaluation(
        UserId interviewerId,
        int score,
        string comments,
        DateTimeOffset date)
    {
        InterviewerId = interviewerId;
        Score = score;
        Comments = comments;
        Date = date;
    }
}
