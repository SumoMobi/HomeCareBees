using Microsoft.AspNetCore.Http;
using System;

namespace Hcb.Insights
{
    public class ReCaptcha
    {
        /// <summary>
        /// Due to the complexities with anti forgery and posting form data from MSTEST to a razor page, I added this method to be 
        /// passed the form data necessary for validation.
        /// </summary>
        /// <param name="reCaptchaTokenInForm">Token received from UI</param>
        /// <param name="httpContext">PageModel's HttpContext</param>
        internal static bool IsValid(string reCaptchaTokenInForm, HttpContext httpContext)
        {
            //Ensure the reCaptcha validation was done and within a few seconds of this post request.  Further that it was for the same token.
            //Is the reCaptcha token in the form?
            if (string.IsNullOrWhiteSpace(reCaptchaTokenInForm))
            {
                return false;
            }
            //Make sure we have the three entries in session.
            string reCaptchaTokenInSession = httpContext.Session.GetString("reCaptchaToken");
            if (string.IsNullOrEmpty(reCaptchaTokenInSession))
            {
                return false;
            }
            string dateTimeValidated = httpContext.Session.GetString("reCaptchaTokenTime");
            if (string.IsNullOrEmpty(dateTimeValidated))
            {
                return false;
            }
            string isTokenValid = httpContext.Session.GetString("isTokenValid");
            if (string.IsNullOrEmpty(isTokenValid))
            {
                return false;
            }

            //Next make sure the values in the session variables are good.
            if (bool.TryParse(isTokenValid, out bool b) == false)
            {
                return false;
            }
            if (b == false)
            {
                return false;
            }

            if (DateTime.TryParse(dateTimeValidated, out DateTime dt) == false)
            {
                return false;
            }
            if (DateTime.UtcNow < dt)
            {
                return false;
            }
            if ((DateTime.UtcNow - dt).TotalMilliseconds > 3000)
            {
                return false;
            }
            if (reCaptchaTokenInForm != reCaptchaTokenInSession)
            {
                return false;
            }

            return true;
        }

    }
}
