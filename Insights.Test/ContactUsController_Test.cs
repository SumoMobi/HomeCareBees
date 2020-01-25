using Hcb.Insights.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Hcb.Insights.Test
{
    [TestClass]
    public class ContactUsController_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            Environment.SetEnvironmentVariable("_RunningUnderMsTest", "true");
            Environment.SetEnvironmentVariable("hcb:reCaptchaSecret", "g3grUoNXNJIy2VqeFczEBLRKIrzVt9M6RRk14qDdLEWV+/y8QEG90afkSvHtBTwA");
        }
        [TestMethod]
        public void Post_InvalidToken()
        {
            ContactUsController controller = new ContactUsController
            {
                ControllerContext = new ControllerContext()
            };
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            string token = "sometesttoken";
            byte[] bytes = Encoding.ASCII.GetBytes(token);
            controller.ControllerContext.HttpContext.Request.Body = new MemoryStream();
            controller.ControllerContext.HttpContext.Request.Body.Write(bytes, 0, bytes.Length);
            controller.ControllerContext.HttpContext.Request.Body.Flush();
            controller.ControllerContext.HttpContext.Request.Body.Position = 0;
            controller.ControllerContext.HttpContext.Session = new TestSession();
            Environment.SetEnvironmentVariable("_IsTokenValid", "false");
            bool result = controller.Post();    //Validate the token
            Assert.AreEqual(false, result);
            //Now make the validation call.
            ModelStateDictionary modelState = new ModelStateDictionary();
            ActionContext actionContext = new ActionContext(controller.ControllerContext.HttpContext, new RouteData(), new PageActionDescriptor(), modelState);
            PageContext pageContext = new PageContext(actionContext);
            Pages.ContactUsModel contactUs = new Pages.ContactUsModel()
            {
                PageContext = pageContext,
            };
            result = contactUs.Validate(token);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void Post_TimedOutToken()
        {
            ContactUsController controller = new ContactUsController
            {
                ControllerContext = new ControllerContext()
            };
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            string token = "sometesttoken";
            byte[] bytes = Encoding.ASCII.GetBytes(token);
            controller.ControllerContext.HttpContext.Request.Body = new MemoryStream();
            controller.ControllerContext.HttpContext.Request.Body.Write(bytes, 0, bytes.Length);
            controller.ControllerContext.HttpContext.Request.Body.Flush();
            controller.ControllerContext.HttpContext.Request.Body.Position = 0;
            controller.ControllerContext.HttpContext.Session = new TestSession();
            Environment.SetEnvironmentVariable("_IsTokenValid", "true");    //Force token to look like a good one.
            bool result = controller.Post();
            Assert.AreEqual(true, result);
            //Now make the validation call.
            ModelStateDictionary modelState = new ModelStateDictionary();
            ActionContext actionContext = new ActionContext(controller.ControllerContext.HttpContext, new RouteData(), new PageActionDescriptor(), modelState);
            PageContext pageContext = new PageContext(actionContext);
            Pages.ContactUsModel contactUs = new Pages.ContactUsModel()
            {
                PageContext = pageContext,
            };
            Thread.Sleep(4000);
            result = contactUs.Validate(token);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void Post_DifferentToken()
        {
            ContactUsController controller = new ContactUsController
            {
                ControllerContext = new ControllerContext()
            };
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            string token = "sometesttoken";
            byte[] bytes = Encoding.ASCII.GetBytes(token);
            controller.ControllerContext.HttpContext.Request.Body = new MemoryStream();
            controller.ControllerContext.HttpContext.Request.Body.Write(bytes, 0, bytes.Length);
            controller.ControllerContext.HttpContext.Request.Body.Flush();
            controller.ControllerContext.HttpContext.Request.Body.Position = 0;
            controller.ControllerContext.HttpContext.Session = new TestSession();
            Environment.SetEnvironmentVariable("_IsTokenValid", "true");    //Force it to look like a valid token
            bool result = controller.Post();
            Assert.AreEqual(true, result);
            //Now make the validation call.
            ModelStateDictionary modelState = new ModelStateDictionary();
            HttpContext contextForPage = new DefaultHttpContext
            {
                Session = controller.HttpContext.Session
            };
            ActionContext actionContext = new ActionContext(contextForPage, new RouteData(), new PageActionDescriptor(), modelState);
            PageContext pageContext = new PageContext(actionContext);

            Pages.ContactUsModel contactUs = new Pages.ContactUsModel()
            {
                PageContext = pageContext,
                Url = new UrlHelper(actionContext)
            };
            result = contactUs.Validate(token + "xyz");
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void Post_AllGood()
        {
            ContactUsController controller = new ContactUsController
            {
                ControllerContext = new ControllerContext()
            };
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            string token = "sometesttoken";
            byte[] bytes = Encoding.ASCII.GetBytes(token);
            controller.ControllerContext.HttpContext.Request.Body = new MemoryStream();
            controller.ControllerContext.HttpContext.Request.Body.Write(bytes, 0, bytes.Length);
            controller.ControllerContext.HttpContext.Request.Body.Flush();
            controller.ControllerContext.HttpContext.Request.Body.Position = 0;
            controller.ControllerContext.HttpContext.Session = new TestSession();
            Environment.SetEnvironmentVariable("_IsTokenValid", "true");
            bool result = controller.Post();
            Assert.AreEqual(true, result);
            //Now make the validation call.
            ModelStateDictionary modelState = new ModelStateDictionary();
            HttpContext contextForPage = new DefaultHttpContext
            {
                Session = controller.HttpContext.Session
            };
            ActionContext actionContext = new ActionContext(contextForPage, new RouteData(), new PageActionDescriptor(), modelState);
            PageContext pageContext = new PageContext(actionContext);
            Pages.ContactUsModel contactUs = new Pages.ContactUsModel()
            {
                PageContext = pageContext,
            };
            result = contactUs.Validate(token);
            Assert.AreEqual(true, result);
        }
    }
}
