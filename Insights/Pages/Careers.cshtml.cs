using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hcb.Insights.Pages
{
    public class CareersModel : PageModel
    {
        public void OnGet()
        {
            ViewData["careersActive"] = "active";
        }
    }
}