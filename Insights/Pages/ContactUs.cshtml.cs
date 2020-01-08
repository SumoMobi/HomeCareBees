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
            StringBuilder sb = new StringBuilder();
            foreach(string key in Request.Form.Keys)
            {
                if (key == "dummy" || key == "__RequestVerificationToken")
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