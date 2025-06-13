namespace ESW.DDD.Workshop.Models
{
    public class Interview
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Guid> InterviewerIds { get; set; } = [];
        public InterviewStatus Status { get; set; }
        public List<Evaluation> Evaluations { get; set; } = [];
    }

    public enum InterviewStatus
    {
        Scheduled,
        InProgress,
        Completed,
        Accepted,
        Rejected,
        InDiscussion
    }
}