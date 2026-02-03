using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoApi.Pages.Products
{
    public class ListModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        public string DisplayCategory => Category switch
        {
            "pitless" => "無機坑電梯",
            "home" => "家庭電梯",
            "bracket" => "角架電梯",
            _ => "產品系列"
        };

        public List<ProductItem> Products { get; set; } = new();

        public void OnGet()
        {
            // Mock data
            Products = new List<ProductItem>
            {
                new ProductItem { Name = "G-10 無機坑旗艦型", Category = "pitless", Description = "不需開挖地坑，安裝靈活。", ImageUrl = "https://images.unsplash.com/photo-1517090504586-fde19ea6066f?auto=format" },
                new ProductItem { Name = "H-20 家庭尊榮款", Category = "home", Description = "專為現代私人別墅設計。", ImageUrl = "https://images.unsplash.com/photo-1517090504586-fde19ea6066f?auto=format" },
                new ProductItem { Name = "B-30 工業角架型", Category = "bracket", Description = "結構穩定，適合室外加裝。", ImageUrl = "https://images.unsplash.com/photo-1517090504586-fde19ea6066f?auto=format" }
            };

            if (!string.IsNullOrEmpty(Category))
            {
                Products = Products.Where(p => p.Category == Category).ToList();
            }
        }
    }

    public class ProductItem
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
