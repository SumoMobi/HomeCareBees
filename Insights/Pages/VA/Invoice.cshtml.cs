using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Hcb.Insights.Pages.VA
{
    public class InvoiceModel : PageModel
    {
        enum fieldNames { id, requestDate, serviceDateTime, responseDate, socialWorker, veteranName, requestedHours, vaToResidenceDistance, cancelDateTime, caregiverCode, notes };

        public class InvoiceLineItems
        {
            public DateTime ServiceDate { get; set; }
            public string Clin { get; set; }
            public decimal Quantity { get; set; }
            public string UnitCode { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Charge { get; set; }
            public string Notes { get; set; }
        }
        public void OnGet()
        {
            DateTime dtInvoice = DateTime.Now.AddMonths(-1);
            string[] rows = System.IO.File.ReadAllLines(@"C:\Users\jean_\OneDrive\Work\HomeCareBees\VA\Requests\2021\requests.txt");
            for (int r = 1; r < rows.Length; r++)
            {   //Skip the header
                string[] fields = rows[r].Split('\t');
                if (fields.Length != 11)
                {
                    throw new ApplicationException($"{fields.Length} number of fields");
                }
                DateTime serviceDateTime = DateTime.Parse(fields[(int)fieldNames.serviceDateTime]);
                if (serviceDateTime.Year != dtInvoice.Year || serviceDateTime.Month != dtInvoice.Month)
                {
                    continue;
                }
                if (fields[(int)fieldNames.cancelDateTime] != "NULL")
                {
                    //Cancellation
                    DateTime cancelDateTime = DateTime.Parse(fields[(int)fieldNames.cancelDateTime]);
                    DateTime cancelDate = new DateTime(cancelDateTime.Year, cancelDateTime.Month, cancelDateTime.Day);    //kind?
                    if (cancelDate < new DateTime(serviceDateTime.Year, serviceDateTime.Month, serviceDateTime.Day))
                    {
                        //Canceled at least the day before.  No charge.
                        continue;
                    }
                    //If canceled the same day, bill 4 hours and no charge for mileage.  Also lump into "canceled" CLIN
                }
            }
        }
    }
}
