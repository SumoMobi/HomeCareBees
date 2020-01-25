using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Hcb.Insights.Test
{
    [TestClass]
    public class ReCaptcha_Test
    {
        Pages.ReCaptchaModel reCaptcha;
        Pages.ContactUsModel contactUs;
        readonly string token = "sometesttoken";

        [TestInitialize]
        public void Initialize()
        {
            Environment.SetEnvironmentVariable("_RunningUnderMsTest", "true");
            Environment.SetEnvironmentVariable("hcb:reCaptchaSecret", "g3grUoNXNJIy2VqeFczEBLRKIrzVt9M6RRk14qDdLEWV+/y8QEG90afkSvHtBTwA");

            HttpContext context = new DefaultHttpContext();
            context.Request.Host = new HostString("localhost");
            byte[] bytes = Encoding.ASCII.GetBytes(token);
            context.Request.Body = new MemoryStream();
            context.Request.Body.Write(bytes, 0, bytes.Length);
            context.Request.Body.Flush();
            context.Request.Body.Position = 0;
            context.Session = new TestSession();
            ModelStateDictionary modelState = new ModelStateDictionary();
            ActionContext actionContext = new ActionContext(context, new RouteData(), new PageActionDescriptor(), modelState);
            PageContext pageContext = new PageContext(actionContext);
            reCaptcha = new Pages.ReCaptchaModel()
            {
                PageContext = pageContext,
            };
            contactUs = new Pages.ContactUsModel()
            {
                PageContext = pageContext,
            };
        }
        [TestMethod]
        public void Post_InvalidToken()
        {

            Environment.SetEnvironmentVariable("_IsTokenValid", "false");
            bool result = reCaptcha.OnPost();    //Validate the token
            Assert.AreEqual(false, result);
            
            //Now make the validation call.
            result = contactUs.Validate(token);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void Post_TimedOutToken()
        {
            Environment.SetEnvironmentVariable("_IsTokenValid", "true");    //Force token to look like a good one.
            bool result = reCaptcha.OnPost();    //Validate the token
            Assert.AreEqual(true, result);
            //Now make the validation call.
            Thread.Sleep(4000);
            result = contactUs.Validate(token);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void Post_DifferentToken()
        {
            Environment.SetEnvironmentVariable("_IsTokenValid", "true");    //Force it to look like a valid token
            bool result = reCaptcha.OnPost();    //Validate the token
            Assert.AreEqual(true, result);
            //Now make the validation call.
            result = contactUs.Validate(token + "xyz");
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void Post_AllGood()
        {
            Environment.SetEnvironmentVariable("_IsTokenValid", "true");
            bool result = reCaptcha.OnPost();    //Validate the token
            Assert.AreEqual(true, result);
            //Now make the validation call.
            result = contactUs.Validate(token);
            Assert.AreEqual(true, result);
        }
    }
}
