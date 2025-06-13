namespace ESW.DDD.WorkshopResults.Domain.Interviews;

internal interface IInterviewRepository
{
    public Interview Get(InterviewId id);

    public void Upsert(Interview interview);
}
