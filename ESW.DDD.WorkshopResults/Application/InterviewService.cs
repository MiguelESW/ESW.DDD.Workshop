using ESW.DDD.WorkshopResults.Domain.Interviews;
using ESW.DDD.WorkshopResults.Domain.Users;

namespace ESW.DDD.WorkshopResults.Application;

internal class InterviewService
{
    private readonly IInterviewRepository _interviewRepository;

    private readonly IUserRepository _userRepository;

    public InterviewService(IInterviewRepository interviewRepository, IUserRepository userRepository)
    {
        _interviewRepository = interviewRepository;
        _userRepository = userRepository;
    }

    public void AddEvaluation(
        InterviewId interviewId,
        Evaluation evaluation)
    {
        var interview = _interviewRepository.Get(interviewId);
        var interviewer = _userRepository.Get(evaluation.InterviewerId);

        interview.AddEvaluation(evaluation);

        _interviewRepository.Upsert(interview);
    }
}
