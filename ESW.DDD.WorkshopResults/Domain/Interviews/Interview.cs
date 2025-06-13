using ESW.DDD.WorkshopResults.Domain.Common;
using ESW.DDD.WorkshopResults.Domain.Common.Abstractions;
using ESW.DDD.WorkshopResults.Domain.Users;

namespace ESW.DDD.WorkshopResults.Domain.Interviews;

internal class Interview : IAggregateRoot
{
    private const int MaxInterviewers = 4;
    private const int MinInterviewers = 1;

    public static Interview Create(
        User candidate,
        List<User> interviewers,
        DateRange slot)
    {
        if (interviewers.Count < MinInterviewers || interviewers.Count > MaxInterviewers)
        {
            throw new ArgumentException(
                $"The number of interviewers must be between {MinInterviewers} and {MaxInterviewers}.", 
                nameof(interviewers));
        }

        if (interviewers.Any(i => i.Type != UserTypes.Interviewer))
        {
            throw new ArgumentException(
                "Invalid interviewers.", 
                nameof(interviewers));
        }

        if (candidate.Type is not UserTypes.Candidate) 
        {
            throw new ArgumentException("Invalid candidate.", nameof(candidate));
        }

        if (slot.HaveStarted)
        {
            throw new ArgumentException("The interview slot must be in the future.", nameof(slot));
        }

        return new(
            id: InterviewId.New(),
            candidateId: candidate.Id,
            slot: slot,
            interviewerIds: interviewers.Select(x => x.Id),
            status: InterviewStatus.Pending,
            evaluations: []);
    }

    public InterviewId Id { get; }
    public UserId CandidateId { get; }
    public DateRange Slot { get; private set; }
    public IEnumerable<UserId> InterviewerIds { get; }
    public InterviewStatus Status { get; private set; }
    private List<Evaluation> _evaluations;
    public IReadOnlyCollection<Evaluation> Evaluations => _evaluations.AsReadOnly();

    public void AddEvaluation(Evaluation newEvaluation)
    {
        EnsureIsInterviewer(newEvaluation.InterviewerId);
        if (Evaluations.Any(evaluation => evaluation.InterviewerId == newEvaluation.InterviewerId))
        {
            throw new ArgumentException(
                "This interviewer has already submitted an evaluation for this interview.", 
                nameof(newEvaluation));
        }
        _evaluations.Add(newEvaluation);
        UpdateInterviewStatus();
    }

    private void UpdateInterviewStatus() 
    {
        if (_evaluations.Count != InterviewerIds.Count())
        {
            return;
        }

        double averageScore = _evaluations.Average(e => e.Score);

        // Count low scores (0 or 1) and high scores (9 or 10)
        int lowScores = _evaluations.Count(e => e.Score <= 1);
        int highScores = _evaluations.Count(e => e.Score >= 9);

        // Determine if majority of interviewers gave low scores
        bool majorityLowScores = lowScores > (InterviewerIds.Count() / 2);

        // Update interview status based on rules
        if (majorityLowScores && highScores > 0)
        {
            // Special case: majority gave low scores but at least one gave high score
            Status = InterviewStatus.InDiscussion;
        }
        else if (majorityLowScores)
        {
            // Majority gave low scores, no high scores
            Status = InterviewStatus.Rejected;
        }
        else if (averageScore >= 6)
        {
            // Average score meets or exceeds threshold
            Status = InterviewStatus.Accepted;
        }
        else
        {
            // Average score below threshold
            Status = InterviewStatus.Rejected;
        }
    }

    public void EnsureIsInterviewer(UserId userId)
    {
        if (!InterviewerIds.Contains(userId))
        {
            throw new ArgumentException(
                "This user is not an interviewer for this interview.", 
                nameof(userId));
        }
    }

    private Interview(
        InterviewId id,
        UserId candidateId,
        DateRange slot,
        IEnumerable<UserId> interviewerIds,
        InterviewStatus status,
        List<Evaluation> evaluations)
    {
        Id = id;
        CandidateId = candidateId;
        Slot = slot;
        InterviewerIds = interviewerIds;
        Status = status;
        _evaluations = evaluations;
    }
}
