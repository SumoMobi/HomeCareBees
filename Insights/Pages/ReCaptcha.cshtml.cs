using Hcb.Insights.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;

namespace Hcb.Insights.Pages
{
    [BindProperties]
    public class ReCaptchaModel : PageModel
    {
        public string Token { get; set; }
        /// <summary>
        /// Called via an AJAX call.
        /// </summary>
        /// <returns>
        /// Responds with ContentResult with either true or false in its content.
        /// </returns>
        public ContentResult OnPost()
        {
            return DoWork(Token);
        }
        /// <summary>
        /// Passing the form fields in here so that the majority of the logic can be unit tested.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal ContentResult DoWork(string token)
        {
            //            StreamReader reader = new StreamReader(Request.Body);
            //            string token = reader.ReadLine();
            bool isTokenValid;
            if (Request.Host.ToString().StartsWith("localhost"))
            {   //Running locally.  Loose the port part.
                isTokenValid = ReCaptchaClient.Verify(token, "localhost").Result;
            }
            else
            {
                isTokenValid = ReCaptchaClient.Verify(token, Request.Host.ToString()).Result;
            }
            if (Environment.GetEnvironmentVariable("hcb:RunningUnderMsTest") == "true")
            {   //Override as the unit test wants you to do.
                if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("hcb:IsTokenValid")) == false)
                {   //Unit test wants to override the token's validity.
                    isTokenValid = bool.Parse(Environment.GetEnvironmentVariable("hcb:IsTokenValid"));
                }
            }
            //In order for hackers not to bypass the reCaptcha check, the POST handler will expect to see proof in session that the token validation passed.
            HttpContext.Session.SetString("reCaptchaToken", token);
            HttpContext.Session.SetString("reCaptchaTokenTime", DateTime.UtcNow.ToString());
            HttpContext.Session.SetString("isTokenValid", isTokenValid.ToString());

            if (isTokenValid == false)
            {
                ModelState.AddModelError("isTokenValid", "invalidToken");
                return new ContentResult()
                {
                    Content = "false",
                    ContentType = "text/plain"
                };
            }

            return new ContentResult()
            {
                Content = "true",
                ContentType = "text/plain"
            };

        }
    }
}