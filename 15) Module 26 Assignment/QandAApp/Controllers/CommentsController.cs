using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QandAApp.Entities;
using QandAApp.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QandAApp.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOnQuestion(Comment comment, int QuestionId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    await _commentService.CreateCommentOnQuestionAsync(comment, userId, QuestionId);
                    return RedirectToAction("Details", "Questions", new { id = QuestionId });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error creating comment.");
                }
            }
            return RedirectToAction("Details", "Questions", new { id = QuestionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOnAnswer(Comment comment, int AnswerId)
        {
            var answer = await _commentService.GetCommentByIdAsync(AnswerId); // To get QuestionId
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    await _commentService.CreateCommentOnAnswerAsync(comment, userId, AnswerId);
                    return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error creating comment.");
                }
            }
            return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (comment.UserId != currentUserId) return Unauthorized();
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Comment comment)
        {
            var existing = await _commentService.GetCommentByIdAsync(comment.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    await _commentService.UpdateCommentAsync(comment, userId);
                    int redirectId = existing.QuestionId ?? existing.AnswerId ?? 0; // Redirect to question
                    return RedirectToAction("Details", "Questions", new { id = redirectId });
                }
                catch (UnauthorizedAccessException)
                {
                    return Unauthorized();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error updating comment.");
                }
            }
            return View(comment);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (comment.UserId != currentUserId) return Unauthorized();
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await _commentService.DeleteCommentAsync(id, userId);
                int redirectId = comment.QuestionId ?? comment.AnswerId ?? 0;
                return RedirectToAction("Details", "Questions", new { id = redirectId });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting comment.");
            }
        }
    }
}