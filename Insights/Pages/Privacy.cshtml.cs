using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hcb.Insights.Pages
{
    public class PrivacyModel : PageModel
    {
        public void OnGet()
        {
            ViewData["privacyActive"] = "active";
        }
    }
}