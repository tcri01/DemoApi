using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DemoApi.Data;
using DemoApi.Models;

namespace DemoApi.Pages.Members
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Member> Members { get;set; } = default!;
        
        [BindProperty]
        public Member Member { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (_context.Members != null)
            {
                Members = await _context.Members.ToListAsync();
            }
        }
    }
}
