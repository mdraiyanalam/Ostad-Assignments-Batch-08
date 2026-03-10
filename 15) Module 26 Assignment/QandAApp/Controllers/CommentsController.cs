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
                    TempData["Success"] = "Comment added successfully!";
                    return RedirectToAction("Details", "Questions", new { id = QuestionId });
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error creating comment: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Comment body is required.";
            }

            return RedirectToAction("Details", "Questions", new { id = QuestionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOnAnswer(Comment comment, int AnswerId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    await _commentService.CreateCommentOnAnswerAsync(comment, userId, AnswerId);

                    // Get QuestionId to redirect properly
                    var answer = await _commentService.GetAnswerByIdAsync(AnswerId); // You need to add this method to ICommentService
                    if (answer == null)
                    {
                        TempData["Error"] = "Answer not found.";
                        return RedirectToAction("Index", "Questions");
                    }

                    TempData["Success"] = "Comment added successfully!";
                    return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error creating comment: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Comment body is required.";
            }

            // Fallback redirect (not ideal, but safe)
            return RedirectToAction("Index", "Questions");
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
            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await _commentService.UpdateCommentAsync(comment, userId);

                var existing = await _commentService.GetCommentByIdAsync(comment.Id);
                int redirectId = existing?.QuestionId ?? existing?.AnswerId ?? 0;

                TempData["Success"] = "Comment updated successfully!";
                return RedirectToAction("Details", "Questions", new { id = redirectId });
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "You can only edit your own comments.";
                return View(comment);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating comment: " + ex.Message);
                return View(comment);
            }
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
                if (comment == null) return NotFound();

                await _commentService.DeleteCommentAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                int redirectId = comment.QuestionId ?? comment.AnswerId ?? 0;
                TempData["Success"] = "Comment deleted successfully!";
                return RedirectToAction("Details", "Questions", new { id = redirectId });
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "You can only delete your own comments.";
                return RedirectToAction("Index", "Questions");
            }
            catch (Exception)
            {
                TempData["Error"] = "Error deleting comment.";
                return RedirectToAction("Index", "Questions");
            }
        }
    }
}