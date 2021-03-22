using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Hcb.Insights.Pages
{
    public class ApplyModel : PageModel
    {
        public void OnPost()
        {
            //TODO: Server-side validation

            if (ReCaptcha.IsValid(Request.Form["reCaptchaToken"], HttpContext) == false)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();            
            foreach (string key in Request.Form.Keys)
            {
                if (key == "__RequestVerificationToken" || key == "reCaptchaToken")
                {
                    continue;
                }
                sb.Append("{ \"" + key + "\":\"" + Request.Form[key] + "\" },");
            }
            Email.Send(
                Request.Form["email"],
                "inquiries@homecarebees.com",
                "Job application",
                "[" + sb.ToString()[..^1] + "]");

            Response.Redirect("ApplyConfirm");

        }
    }
}
