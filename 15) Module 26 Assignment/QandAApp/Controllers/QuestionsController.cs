using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QandAApp.Entities;
using QandAApp.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QandAApp.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly ICommentService _commentService;

        public QuestionsController(
            IQuestionService questionService,
            IAnswerService answerService,
            ICommentService commentService)
        {
            _questionService = questionService;
            _answerService = answerService;
            _commentService = commentService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return View(questions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question question)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    await _questionService.CreateQuestionAsync(question, userId);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error creating question.");
                }
            }
            return View(question);
        }

        public async Task<IActionResult> Details(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            // Explicit loading using exposed Repository from service
            var dbContext = _questionService.Repository.Context;

            await dbContext.Entry(question).Reference(q => q.User).LoadAsync();
            await dbContext.Entry(question).Collection(q => q.Answers).LoadAsync();
            await dbContext.Entry(question).Collection(q => q.Comments).LoadAsync();

            foreach (var answer in question.Answers ?? Enumerable.Empty<Answer>())
            {
                await dbContext.Entry(answer).Reference(a => a.User).LoadAsync();
                await dbContext.Entry(answer).Collection(a => a.Comments).LoadAsync();
            }

            foreach (var comment in question.Comments ?? Enumerable.Empty<Comment>())
            {
                await dbContext.Entry(comment).Reference(c => c.User).LoadAsync();
            }

            return View(question);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (question.UserId != currentUserId) return Unauthorized();

            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    await _questionService.UpdateQuestionAsync(question, userId);
                    return RedirectToAction(nameof(Details), new { id = question.Id });
                }
                catch (UnauthorizedAccessException)
                {
                    return Unauthorized();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error updating question.");
                }
            }
            return View(question);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (question.UserId != currentUserId) return Unauthorized();

            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await _questionService.DeleteQuestionAsync(id, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting question.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptAnswer(int questionId, int answerId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await _questionService.AcceptAnswerAsync(questionId, answerId, userId);
                return RedirectToAction(nameof(Details), new { id = questionId });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest("Error accepting answer.");
            }
        }
    }
}