using QandAApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QandAApp.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetCommentsByQuestionIdAsync(int questionId);
        Task<IEnumerable<Comment>> GetCommentsByAnswerIdAsync(int answerId);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<Answer?> GetAnswerByIdAsync(int id);
        Task CreateCommentOnQuestionAsync(Comment comment, string userId, int questionId);
        Task CreateCommentOnAnswerAsync(Comment comment, string userId, int answerId);
        Task UpdateCommentAsync(Comment comment, string userId);
        Task DeleteCommentAsync(int id, string userId);
    }
}