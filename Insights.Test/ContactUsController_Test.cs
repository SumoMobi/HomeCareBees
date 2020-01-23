using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hcb.Insights.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;

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
            controller.ControllerContext.HttpContext.Session = new TestSession();
            Environment.SetEnvironmentVariable("_IsTokenValid", "true");    //Force it to look like a valid token
            bool result = controller.Post();
            Assert.AreEqual(true, result);
            //Now make the validation call.
            ModelStateDictionary modelState = new ModelStateDictionary();
            HttpContext contextForPage = new DefaultHttpContext();
            contextForPage.Session = controller.HttpContext.Session;
            ActionContext actionContext = new ActionContext(contextForPage, new RouteData(), new PageActionDescriptor(), modelState);
            PageContext pageContext = new PageContext(actionContext);
            //pageContext.HttpContext.Request.Body = new MemoryStream();
            //bytes = Encoding.ASCII.GetBytes("promotionCode=&firstName=Jean&lastName=Minnaar&phoneNumber=4802086807&patientAgeGroup=76%2B&morningSunday=on&morningMonday=on&emailAddress=jean.minnaar%40homecarebees.com&inquiry=fffffffffffff&reCaptchaToken=03AOLTBLSWS0kCcPPc6Y6sLE6tBX28cfp2pSMb1qWt23FTIUtrdGCz3ZjeNQSoQNZVzL9hamZpcqEOxv8pNdNyikDGldu6LYDr4m280fo7UGLxH1vzLSj2upfYGbCNxlfncoq8e1YSRabbq1aaVfbgDOrZlC-pVes8jEdrlyLBAH1Qp8SGv8pzB2Bk7uzDpMmLmx0sr4uEbT-MAmeHpx9ENI6ZuSHdRnsliYc2sN-d623msOmc1P8e6X6vnIJ9B48A0KRP_vjJWNrn1ybgwkhi18bGG4T0zXLOaHz02dD9GR11-4_uV4xTXKN5aCiWzdntBSF7zQ8j50e1TV66j89dAPlsi8uXyM3JAmKZdJimIy3hx6_Z7I4WLtA30zhs-IJQpOoQNhJ9n0cUfeAZ1KtRY2SVcJfbNAmN9JcnY5wldMUDrOfNJQrSpwG_hO-nd_W-XeEpLaAI4d90QmpUAYA8bFWS3QA9JsnapdTM09xwQ9HtrJ6DYVIcvGN_85dNZr2n6iyVTp3RY5bCzUwWlulSsTjWt014SUIjsQ&__RequestVerificationToken=CfDJ8Bo_msqJfLdAkBTKkmWwgv5I4s2uVJdnhNcO1X98IvwOJqYA-vMEZHCFCc_RuEYCAj4Vo2Z5VLYWla1T0QCWzTOnDYW-rY0dGHDTUZ9ds-GxLH1wbie4oa11bYwWWUU53EXgu6yzvUUcarSa3ohtsDc");
            //bytes = Encoding.ASCII.GetBytes("reCaptchaToken=somethingelse");
            //pageContext.HttpContext.Request.Body.Write(bytes, 0, bytes.Length);
            //pageContext.HttpContext.Request.Body.Flush();
            //pageContext.HttpContext.Request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

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
            controller.ControllerContext.HttpContext.Session = new TestSession();
            Environment.SetEnvironmentVariable("_IsTokenValid", "true");
            bool result = controller.Post();
            Assert.AreEqual(true, result);
            //Now make the form post call.
            ModelStateDictionary modelState = new ModelStateDictionary();
            HttpContext contextForPage = new DefaultHttpContext();
            contextForPage.Session = controller.HttpContext.Session;
            ActionContext actionContext = new ActionContext(contextForPage, new RouteData(), new PageActionDescriptor(), modelState);
            PageContext pageContext = new PageContext(actionContext);
            //pageContext.HttpContext.Request.Body = new MemoryStream();
            //bytes = Encoding.ASCII.GetBytes("reCaptchaToken=sometesttoken");
            //pageContext.HttpContext.Request.Body.Write(bytes, 0, bytes.Length);
            //pageContext.HttpContext.Request.Body.Flush();

            Pages.ContactUsModel contactUs = new Pages.ContactUsModel()
            {
                PageContext = pageContext,
            };
            result = contactUs.Validate(token);
            Assert.AreEqual(true, result);
        }
    }
    /// <summary>
    /// Use this to mock session of the HttpContext in your unit tests.
    /// </summary>
    public class TestSession : ISession
    {
        public TestSession()
        {
            Values = new Dictionary<string, byte[]>();
        }

        public string Id
        {
            get
            {
                return "session_id";
            }
        }

        public bool IsAvailable
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<string> Keys
        {
            get { return Values.Keys; }
        }

        public Dictionary<string, byte[]> Values { get; set; }

        public void Clear()
        {
            Values.Clear();
        }

        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync()
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            Values.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            if (Values.ContainsKey(key))
            {
                Remove(key);
            }
            Values.Add(key, value);
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (Values.ContainsKey(key))
            {
                value = Values[key];
                return true;
            }
            value = new byte[0];
            return false;
        }
    }
}
