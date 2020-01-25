using Hcb.Insights.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Hcb.Insights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        // POST: api/ContactUs
        [HttpPost]
        public bool Post()
        {
            StreamReader reader = new StreamReader(Request.Body);
            string token = reader.ReadLine();
            bool isTokenValid;
            if (Request.Host.ToString().StartsWith("localhost"))
            {   //Running locally.  Loose the port part.
                isTokenValid = ReCaptchaClient.Verify(token, "localhost").Result;
            }
            else
            {
                isTokenValid = ReCaptchaClient.Verify(token, Request.Host.ToString()).Result;
            }
            if (Environment.GetEnvironmentVariable("_RunningUnderMsTest") == "true")
            {   //Override as the unit test wants you to do.
                if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("_IsTokenValid")) == false)
                {   //Unit test wants to override the token's validity.
                    isTokenValid = bool.Parse(Environment.GetEnvironmentVariable("_IsTokenValid"));
                }
            }
            //In order for hackers not to bypass the reCaptcha check, the POST handler will expect to see proof in session that the token validation passed.
            HttpContext.Session.SetString("reCaptchaToken", token);
            HttpContext.Session.SetString("reCaptchaTokenTime", DateTime.UtcNow.ToString());
            HttpContext.Session.SetString("isTokenValid", isTokenValid.ToString());

            return isTokenValid;
        }
    }
}
