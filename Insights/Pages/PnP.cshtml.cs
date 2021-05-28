using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace Hcb.Insights.Pages
{
    public class PnPModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (AllowPageRequest())
            {
                return Page();
            }
            if (Request.Query.Count == 0)
            {
                return Page();
            }
            if (Request.Query.Count != 1)
            {
                return Page();
            }
            if (Request.Query.ContainsKey("key") == false)
            {
                return Page();
            }
            List<string> keys = new List<string>()
            {
                "cbb032ff-cf4c-4fb0-bebe-97e828c53d82", //Jean
                "98f0e9f7-07d1-48d4-8f1d-bbcffaeac6e3", //Isaac
                "88306556-e351-487c-8583-03f64cfb7cc4", //VA-1
                "863d2205-9f4f-49f6-a339-c71da66a0b35", //VA-2
                "78d94054-06ac-4ad5-b302-f58acc7a9568",
                "df237c35-2a28-4aad-80cd-052286f6b140",
                "2cec5937-79bc-4f85-9606-3707e5eca31e",
                "480cdbdd-8d97-4e9f-8bd0-94832a589f9e",
                "1966544a-7fdd-4098-8ae6-7fcb80d7afa2",
                "4cb44744-ce39-420f-a874-815f6e0dee09",
                "e3b9de0f-b4fe-4047-a6b5-19e2fb6c4a47"
            };
            if (keys.Contains(Request.Query["key"]) == false)
            {
                return Page();
            }
            HttpContext.Session.Set("pnp", Encoding.UTF8.GetBytes("pnp"));
            return new RedirectToPageResult("pnp");
        }
        public FileStreamResult OnGetPage(int pageNumber)
        {
            if (AllowPageRequest() == false)
            {
                return null;
            }
            string url = $"http{(Request.IsHttps ? "s": "")}://{Request.Host}/images/PnP/PoliciesAndProcedures1024_{pageNumber}.jpg";
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
            HttpResponseMessage httpResponseMessage = httpClient.GetAsync("").Result;
            System.IO.Stream stream = httpResponseMessage.Content.ReadAsStreamAsync().Result;
            return File(stream, "image/jpeg");
        }
        private bool AllowPageRequest()
        {
            if (this.HttpContext.Session.IsAvailable == false)
            {
                return false;
            }
            if (Request.IsHttps == false)
            {
                return false;
            }
            byte[] value = new byte[20];
            if (HttpContext.Session.TryGetValue("pnp", out value) == false)
            {
                return false;
            }
            string strVal = Encoding.UTF8.GetString(value);
            if (Request.Headers["Referer"].ToString().ToLower() != "https://www.homecarebees.com/pnp")
            {
                if (Debugger.IsAttached)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
