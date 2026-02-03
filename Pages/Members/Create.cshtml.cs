using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DemoApi.Data;
using DemoApi.Models;
using DemoApi.Services;

namespace DemoApi.Pages.Members
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAIService _aiService;

        public CreateModel(ApplicationDbContext context, IAIService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Member Member { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Members == null || Member == null)
            {
                return Page();
            }

            _context.Members.Add(Member);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        // AI 輔助功能: 產生備註建議
        public async Task<JsonResult> OnGetSuggestNotesAsync(string name, string bio)
        {
            var suggestion = await _aiService.SuggestNotesAsync(name, bio);
            return new JsonResult(new { success = true, suggestion });
        }

        // AI 輔助功能: 描述補全
        public async Task<JsonResult> OnGetCompleteBioAsync(string currentBio)
        {
            var completed = await _aiService.CompleteDescriptionAsync(currentBio);
            return new JsonResult(new { success = true, completed });
        }
    }
}
