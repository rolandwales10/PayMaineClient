using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bal = LUPC.BusinessAreaLayer;
using vm = LUPC.ViewModels;

namespace LUPC.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult RouteToBank(int? ClientPaymentId)
        {
            var pmc = new vm.Vm_PayMaineRequestCollection();
            if (ClientPaymentId == null)
            {
                return Content("Error: Payment/Request needs a client payment id");
            }
            pmc.payMaineRequest.ClientPaymentId = (int)ClientPaymentId;
            var balpr = new bal.Bal_Payment();
            balpr.Checkout(pmc);
            if (pmc.messages.Count == 0)
            {
                string url = ConfigurationManager.AppSettings["PayMaine_Request_Collect"]
                    + "?ClientApplicationId=" + Utilities.Globals.applicationId + "&ClientPaymentId="
                    + pmc.payMaineRequest.ClientPaymentId;
                return Redirect(url);
            }
            else return Content(pmc.messages[0].content);
        }
    }
}