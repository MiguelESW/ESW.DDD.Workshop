using ESW.DDD.Workshop.Models;

namespace ESW.DDD.Workshop.Repositories
{
    public interface IEvaluationRepository : IRepository<Evaluation>
    {
        Task<IEnumerable<Evaluation>> GetEvaluationsByInterviewIdAsync(Guid interviewId);
    }
}