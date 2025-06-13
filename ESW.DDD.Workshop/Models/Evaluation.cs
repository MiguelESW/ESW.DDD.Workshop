using System;

namespace ESW.DDD.Workshop.Models
{
    public class Evaluation
    {
        public Guid Id { get; set; }
        public Guid InterviewId { get; set; }
        public Guid InterviewerId { get; set; }
        public int Score { get; set; } 
        public string Comments { get; set; }
        public DateTime EvaluationDate { get; set; }
    }
}