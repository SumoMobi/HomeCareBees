using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hcb.Insights.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (Request.Path != "/")
            {
                return RedirectToPagePermanent("");
            }
            return Page();
        }
    }
}
