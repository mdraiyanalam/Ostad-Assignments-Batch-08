using Microsoft.EntityFrameworkCore;
using QandAApp.Entities;
using QandAApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QandAApp.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IRepository<Answer> _answerRepo;
        private readonly IRepository<Comment> _commentRepo;

        public AnswerService(
            IRepository<Answer> answerRepo,
            IRepository<Comment> commentRepo)
        {
            _answerRepo = answerRepo;
            _commentRepo = commentRepo;
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            var answers = await _answerRepo.FindAsync(a => a.QuestionId == questionId);
            foreach (var a in answers)
            {
                await _answerRepo.Context.Entry(a).Reference(a => a.User).LoadAsync();
                await _answerRepo.Context.Entry(a).Collection(a => a.Comments).LoadAsync();
            }
            return answers;
        }

        public async Task<Answer?> GetAnswerByIdAsync(int id)
        {
            var answer = await _answerRepo.GetByIdAsync(id);
            if (answer == null) return null;

            await _answerRepo.Context.Entry(answer).Reference(a => a.User).LoadAsync();
            await _answerRepo.Context.Entry(answer).Reference(a => a.Question).LoadAsync();
            await _answerRepo.Context.Entry(answer).Collection(a => a.Comments).LoadAsync();

            foreach (var comment in answer.Comments ?? Enumerable.Empty<Comment>())
            {
                await _answerRepo.Context.Entry(comment).Reference(c => c.User).LoadAsync();
            }

            return answer;
        }

        public async Task CreateAnswerAsync(Answer answer, string userId)
        {
            answer.UserId = userId;
            await _answerRepo.AddAsync(answer);
            await _answerRepo.SaveChangesAsync();
        }

        public async Task UpdateAnswerAsync(Answer answer, string userId)
        {
            var existing = await GetAnswerByIdAsync(answer.Id);
            if (existing == null || existing.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only edit your own answers.");
            }

            existing.Body = answer.Body;
            _answerRepo.Update(existing);
            await _answerRepo.SaveChangesAsync();
        }

        public async Task DeleteAnswerAsync(int id, string userId)
        {
            var answer = await GetAnswerByIdAsync(id);
            if (answer == null || answer.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own answers.");
            }

            // Delete all comments on this answer
            foreach (var comment in answer.Comments ?? Enumerable.Empty<Comment>())
            {
                _commentRepo.Delete(comment);
            }

            _answerRepo.Delete(answer);
            await _answerRepo.SaveChangesAsync();
        }
    }
}