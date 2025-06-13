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
    public IEnumerable<Evaluation> Evaluations { get; }

    private Interview(
        InterviewId id,
        UserId candidateId,
        DateRange slot,
        IEnumerable<UserId> interviewerIds,
        InterviewStatus status,
        IEnumerable<Evaluation> evaluations)
    {
        Id = id;
        CandidateId = candidateId;
        Slot = slot;
        InterviewerIds = interviewerIds;
        Status = status;
        Evaluations = evaluations;
    }
}
