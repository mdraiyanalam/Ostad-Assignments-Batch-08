using QandAApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QandAApp.Services
{
    public interface IAnswerService
    {
        Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int questionId);
        Task<Answer?> GetAnswerByIdAsync(int id);
        Task CreateAnswerAsync(Answer answer, string userId);
        Task UpdateAnswerAsync(Answer answer, string userId);
        Task DeleteAnswerAsync(int id, string userId);
    }
}