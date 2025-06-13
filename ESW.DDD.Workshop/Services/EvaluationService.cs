using ESW.DDD.Workshop.Models;
using ESW.DDD.Workshop.Repositories;

namespace ESW.DDD.Workshop.Services
{
    public class EvaluationService
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IUserRepository _userRepository;

        public EvaluationService(
            IInterviewRepository interviewRepository,
            IEvaluationRepository evaluationRepository,
            IUserRepository userRepository)
        {
            _interviewRepository = interviewRepository;
            _evaluationRepository = evaluationRepository;
            _userRepository = userRepository;
        }

        public async Task<Evaluation> SubmitEvaluation(Guid interviewId, Guid interviewerId, int score, string comments)
        {
            // Validate score range
            if (score < 0 || score > 10)
            {
                throw new ArgumentException("Score must be between 0 and 10");
            }

            // Validate interviewer exists and is an interviewer
            var interviewer = await _userRepository.GetByIdAsync(interviewerId);
            if (interviewer == null || interviewer.Type != UserType.Interviewer)
            {
                throw new ArgumentException("User not found or is not an interviewer");
            }

            // Validate interview exists
            var interview = await _interviewRepository.GetByIdAsync(interviewId);
            if (interview == null)
            {
                throw new ArgumentException("Interview not found");
            }

            // Validate interviewer is assigned to this interview
            if (!interview.InterviewerIds.Contains(interviewerId))
            {
                throw new ArgumentException("This interviewer is not assigned to this interview");
            }

            // Create and save the evaluation
            var evaluation = new Evaluation
            {
                Id = Guid.NewGuid(),
                InterviewId = interviewId,
                InterviewerId = interviewerId,
                Score = score,
                Comments = comments,
                EvaluationDate = DateTime.UtcNow
            };

            await _evaluationRepository.AddAsync(evaluation);

            // Check if all interviewers have submitted evaluations
            var evaluations = await _evaluationRepository.GetEvaluationsByInterviewIdAsync(interviewId);
            if (evaluations.Count() >= interview.InterviewerIds.Count)
            {
                // All evaluations received, determine interview status
                await UpdateInterviewStatusAsync(interviewId);
            }

            return evaluation;
        }

        private async Task UpdateInterviewStatusAsync(Guid interviewId)
        {
            var interview = await _interviewRepository.GetByIdAsync(interviewId);
            if (interview == null)
            {
                throw new ArgumentException("Interview not found");
            }

            var evaluations = await _evaluationRepository.GetEvaluationsByInterviewIdAsync(interviewId);
            if (evaluations == null || !evaluations.Any())
            {
                throw new InvalidOperationException("No evaluations found for this interview");
            }

            // Calculate the average score
            double averageScore = evaluations.Average(e => e.Score);

            // Count low scores (0 or 1) and high scores (9 or 10)
            int lowScores = evaluations.Count(e => e.Score <= 1);
            int highScores = evaluations.Count(e => e.Score >= 9);

            // Determine if majority of interviewers gave low scores
            bool majorityLowScores = lowScores > (interview.InterviewerIds.Count / 2);

            // Update interview status based on rules
            if (majorityLowScores && highScores > 0)
            {
                // Special case: majority gave low scores but at least one gave high score
                interview.Status = InterviewStatus.InDiscussion;
            }
            else if (majorityLowScores)
            {
                // Majority gave low scores, no high scores
                interview.Status = InterviewStatus.Rejected;
            }
            else if (averageScore >= 6.5)
            {
                // Average score meets or exceeds threshold
                interview.Status = InterviewStatus.Accepted;
            }
            else
            {
                // Average score below threshold
                interview.Status = InterviewStatus.Rejected;
            }

            await _interviewRepository.UpdateAsync(interview);
        }

        public async Task<InterviewStatus> GetInterviewStatusAsync(Guid interviewId)
        {
            var interview = await _interviewRepository.GetByIdAsync(interviewId);
            if (interview == null)
            {
                throw new ArgumentException("Interview not found");
            }

            return interview.Status;
        }
    }
}