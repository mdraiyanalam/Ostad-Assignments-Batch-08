using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QandAApp.Entities;
using QandAApp.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QandAApp.Controllers
{
    [Authorize]
    public class AnswersController : Controller
    {
        private readonly IAnswerService _answerService;

        public AnswersController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Answer answer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    await _answerService.CreateAnswerAsync(answer, userId);
                    return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error creating answer.");
                }
            }
            return RedirectToAction("Details", "Questions", new { id = answer.QuestionId }); // Redirect back on error
        }

        public async Task<IActionResult> Edit(int id)
        {
            var answer = await _answerService.GetAnswerByIdAsync(id);
            if (answer == null) return NotFound();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (answer.UserId != currentUserId) return Unauthorized();
            return View(answer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Answer answer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    await _answerService.UpdateAnswerAsync(answer, userId);
                    return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
                }
                catch (UnauthorizedAccessException)
                {
                    return Unauthorized();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error updating answer.");
                }
            }
            return View(answer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var answer = await _answerService.GetAnswerByIdAsync(id);
            if (answer == null) return NotFound();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (answer.UserId != currentUserId) return Unauthorized();
            return View(answer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var answer = await _answerService.GetAnswerByIdAsync(id);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await _answerService.DeleteAnswerAsync(id, userId);
                return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting answer.");
            }
        }
    }
}