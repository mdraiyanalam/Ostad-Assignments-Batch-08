using Microsoft.EntityFrameworkCore;
using QandAApp.Entities;
using QandAApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QandAApp.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<Answer> _answerRepo;     // ← ADD THIS
        private readonly IRepository<Comment> _commentRepo;

        public QuestionService(
            IRepository<Question> questionRepo,
            IRepository<Answer> answerRepo,                         // ← ADD THIS
            IRepository<Comment> commentRepo)
        {
            _questionRepo = questionRepo;
            _answerRepo = answerRepo;                             // ← ADD THIS
            _commentRepo = commentRepo;
        }

        // Expose repository for controller to access Context safely
        public IRepository<Question> Repository => _questionRepo;

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            var questions = await _questionRepo.GetAllAsync();
            foreach (var q in questions)
            {
                await _questionRepo.Context.Entry(q).Reference(q => q.User).LoadAsync();
                await _questionRepo.Context.Entry(q).Collection(q => q.Answers).LoadAsync();
                await _questionRepo.Context.Entry(q).Collection(q => q.Comments).LoadAsync();
            }
            return questions;
        }

        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            var question = await _questionRepo.GetByIdAsync(id);
            if (question == null) return null;

            await _questionRepo.Context.Entry(question).Reference(q => q.User).LoadAsync();
            await _questionRepo.Context.Entry(question).Collection(q => q.Answers).LoadAsync();
            await _questionRepo.Context.Entry(question).Collection(q => q.Comments).LoadAsync();

            foreach (var answer in question.Answers ?? Enumerable.Empty<Answer>())
            {
                await _questionRepo.Context.Entry(answer).Reference(a => a.User).LoadAsync();
                await _questionRepo.Context.Entry(answer).Collection(a => a.Comments).LoadAsync();
            }

            foreach (var comment in question.Comments ?? Enumerable.Empty<Comment>())
            {
                await _questionRepo.Context.Entry(comment).Reference(c => c.User).LoadAsync();
            }

            return question;
        }

        public async Task CreateQuestionAsync(Question question, string userId)
        {
            question.UserId = userId;
            await _questionRepo.AddAsync(question);
            await _questionRepo.SaveChangesAsync();
        }

        public async Task UpdateQuestionAsync(Question question, string userId)
        {
            var existing = await _questionRepo.GetByIdAsync(question.Id);
            if (existing == null || existing.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only edit your own questions.");
            }

            existing.Title = question.Title;
            existing.Body = question.Body;

            _questionRepo.Update(existing);
            await _questionRepo.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(int id, string userId)
        {
            var question = await GetQuestionByIdAsync(id);
            if (question == null || question.UserId != userId)
            {
                throw new UnauthorizedAccessException("Not your question.");
            }

            // Delete all comments directly on the question
            foreach (var comment in question.Comments ?? Enumerable.Empty<Comment>())
            {
                _commentRepo.Delete(comment);
            }

            // Delete comments on all answers + the answers themselves
            foreach (var answer in question.Answers ?? Enumerable.Empty<Answer>())
            {
                foreach (var comment in answer.Comments ?? Enumerable.Empty<Comment>())
                {
                    _commentRepo.Delete(comment);
                }

                _answerRepo.Delete(answer);          // ← FIXED HERE: use _answerRepo
            }

            _questionRepo.Delete(question);
            await _questionRepo.SaveChangesAsync();
        }

        public async Task AcceptAnswerAsync(int questionId, int answerId, string userId)
        {
            var question = await GetQuestionByIdAsync(questionId);
            if (question == null || question.UserId != userId)
            {
                throw new UnauthorizedAccessException("Not your question.");
            }

            question.AcceptedAnswerId = answerId;

            var answer = question.Answers?.FirstOrDefault(a => a.Id == answerId);
            if (answer != null)
            {
                answer.IsAccepted = true;
            }

            _questionRepo.Update(question);
            await _questionRepo.SaveChangesAsync();
        }
    }
}