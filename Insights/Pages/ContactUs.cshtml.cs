using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Hcb.Insights.Pages
{
    public class ContactUsModel : PageModel
    {
        public void OnGet()
        {
            ViewData["thankYouDisplayStyle"] = "none";
            ViewData["contactUsActive"] = "active";
        }
        public void OnPost()
        {
            Email.Send(
                Request.Form["emailAddress"],
                "inquiries@homecarebees.com",
                $"First Name: {Request.Form["firstName"]}\nLast Name: {Request.Form["lastName"]}\nPhone Number: {Request.Form["phoneNumber"]}\nCall Times: {string.Join(',', Request.Form["callTimes"])}\nInquiry:\n{Request.Form["inquiry"]}");

            ViewData["thankYouDisplayStyle"] = "";
        }
    }
}