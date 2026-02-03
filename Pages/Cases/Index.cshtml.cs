using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoApi.Pages.Cases
{
    public class IndexModel : PageModel
    {
        public List<CaseItem> Cases { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 9;
        public int TotalPages { get; set; }

        public void OnGet()
        {
            // 模擬大量的實績案例數據
            var allCases = Enumerable.Range(1, 45).Select(i => new CaseItem
            {
                Id = i,
                Title = $"櫻花實績案例 #{i}",
                Location = i % 2 == 0 ? "台北市 / 住宅大樓" : "台中市 / 私人別墅",
                Description = "本案採用的技術優化了建築空間，提供了優雅且安全的升降體驗。",
                ImageUrl = $"https://images.unsplash.com/photo-1517090504586-fde19ea6066f?q=80&w=600&auto=format&sig={i}"
            }).ToList();

            TotalPages = (int)Math.Ceiling(allCases.Count / (double)PageSize);
            
            // 確保頁碼正確
            if (CurrentPage < 1) CurrentPage = 1;
            if (CurrentPage > TotalPages) CurrentPage = TotalPages;

            Cases = allCases
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }

    public class CaseItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
