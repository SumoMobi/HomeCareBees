using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hcb.Insights.Pages
{
    public class CheckboxIdAndLabel
    {
        public string Id { get; set; }
        public string Label { get; set; }
    }
    public class BestTimeTocall
    {
        public string DayLabel { get; set; }
        public List<CheckboxIdAndLabel> CheckboxIdsAndLabels { get; set; }
    }
    public class ContactUsModel : PageModel
    {
        public List<BestTimeTocall> BestTimesToCall = new List<BestTimeTocall>();

        public void OnGet()
        {
            ViewData["contactUsActive"] = "active";

            SetUpBestTimesToCall();
        }
        public void OnPost()
        {
            //Ensure the reCaptcha validation was done and within a few seconds of this post request.  Further that it was for the same token.
            //First make sure we have the three entries in session.
            string reCaptchaToken = HttpContext.Session.GetString("reCaptchaToken");
            if (string.IsNullOrEmpty(reCaptchaToken))
            {
                return;
            }
            string dateTimeValidated = HttpContext.Session.GetString("reCaptchaTokenTime");
            if (string.IsNullOrEmpty(dateTimeValidated))
            {
                return;
            }
            string isTokenValid = HttpContext.Session.GetString("isTokenValid");
            if (string.IsNullOrEmpty(isTokenValid))
            {
                return;
            }

            //Next make sure the values in the session variables are good.
            if (bool.TryParse(isTokenValid, out bool b) == false)
            {
                return;
            }
            if (b == false)
            {
                return;
            }

            if (DateTime.TryParse(dateTimeValidated, out DateTime dt) == false)
            {
                return;
            }
            if (DateTime.UtcNow < dt)
            {
                return;
            }
            if ((DateTime.UtcNow - dt).TotalMilliseconds > 3000)
            {
                return;
            }

            if (Request.Form["reCaptchaToken"] != reCaptchaToken)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach(string key in Request.Form.Keys)
            {
                if (key == "__RequestVerificationToken" || key == "reCaptchaToken")
                {
                    continue;
                }
                sb.Append($"{key}[{Request.Form[key]}]\n");
            }
            Email.Send(
                Request.Form["emailAddress"],
                "inquiries@homecarebees.com",
                sb.ToString());

            Response.Redirect("Confirm");
        }
        private void SetUpBestTimesToCall()
        {
            foreach (string dayOfWeek in Enum.GetNames(typeof(DayOfWeek)))
            {
                List<CheckboxIdAndLabel> checkboxIdsAndLabels = new List<CheckboxIdAndLabel>()
                {
                    new CheckboxIdAndLabel
                    {
                        Id = $"morning{dayOfWeek}",
                        Label = "Morning"
                    },
                    new CheckboxIdAndLabel
                    {
                        Id = $"afternoon{dayOfWeek}",
                        Label = "Afternoon"
                    },
                    new CheckboxIdAndLabel
                    {
                        Id = $"evening{dayOfWeek}",
                        Label = "Evening"
                    }
                };
                BestTimesToCall.Add(new BestTimeTocall
                {
                    DayLabel = dayOfWeek,
                    CheckboxIdsAndLabels = checkboxIdsAndLabels
                });
            }
        }
    }
}