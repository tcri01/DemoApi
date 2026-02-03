using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DemoApi.Data;
using DemoApi.Models;
using DemoApi.Services;

namespace DemoApi.Pages.Members
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAIService _aiService;

        public EditModel(ApplicationDbContext context, IAIService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        [BindProperty]
        public Member Member { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member =  await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }
            Member = member;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(Member.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MemberExists(int id)
        {
          return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<JsonResult> OnGetSuggestContentAsync(string name, string company)
        {
            var suggestion = await _aiService.SuggestNotesAsync(name, company);
            return new JsonResult(new { success = true, suggestion });
        }

        public async Task<JsonResult> OnGetCompleteContentAsync(string currentText)
        {
            var completed = await _aiService.CompleteDescriptionAsync(currentText);
            return new JsonResult(new { success = true, completed });
        }
    }
}
