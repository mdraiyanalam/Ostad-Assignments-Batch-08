using Microsoft.EntityFrameworkCore;
using QandAApp.Entities;
using QandAApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QandAApp.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _repository;

        public CommentService(IRepository<Comment> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByQuestionIdAsync(int questionId)
        {
            return await _repository.FindAsync(
                c => c.QuestionId == questionId,
                c => c.User!
            );
        }

        public async Task<IEnumerable<Comment>> GetCommentsByAnswerIdAsync(int answerId)
        {
            return await _repository.FindAsync(
                c => c.AnswerId == answerId,
                c => c.User!
            );
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id, c => c.User!);
        }

        // New method: Get Answer with Question included (for redirect in CreateOnAnswer)
        public async Task<Answer?> GetAnswerByIdAsync(int id)
        {
            return await _repository.Context.Set<Answer>()
                .Include(a => a.Question)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task CreateCommentOnQuestionAsync(Comment comment, string userId, int questionId)
        {
            comment.UserId = userId;
            comment.QuestionId = questionId;
            await _repository.AddAsync(comment);
            await _repository.SaveChangesAsync();
        }

        public async Task CreateCommentOnAnswerAsync(Comment comment, string userId, int answerId)
        {
            comment.UserId = userId;
            comment.AnswerId = answerId;
            await _repository.AddAsync(comment);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Comment comment, string userId)
        {
            var existing = await GetCommentByIdAsync(comment.Id);
            if (existing == null || existing.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only edit your own comments.");
            }

            existing.Body = comment.Body;
            _repository.Update(existing);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int id, string userId)
        {
            var comment = await GetCommentByIdAsync(id);
            if (comment == null || comment.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own comments.");
            }

            _repository.Delete(comment);
            await _repository.SaveChangesAsync();
        }
    }
}