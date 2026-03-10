using QandAApp.Entities;
using QandAApp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QandAApp.Services
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<Question?> GetQuestionByIdAsync(int id);
        Task CreateQuestionAsync(Question question, string userId);
        Task UpdateQuestionAsync(Question question, string userId);
        Task DeleteQuestionAsync(int id, string userId);
        Task AcceptAnswerAsync(int questionId, int answerId, string userId);

        // Expose repository so controller can safely access DbContext for loading
        IRepository<Question> Repository { get; }
    }
}