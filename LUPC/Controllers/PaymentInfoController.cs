using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using nt = Newtonsoft.Json;
using bal = LUPC.BusinessAreaLayer;
using vm = LUPC.ViewModels;

namespace LUPC.Controllers
{
    public class PaymentInfoController : Controller
    {
        // GET: PaymentInfo
        public ActionResult Edit(int? TrackingNbr)
        {
            var pr = new vm.Vm_PayMaineRequestCollection();
            try
            {
                var bal = new bal.Bal_TrackingNbr();
                if (TrackingNbr == null)
                    vm.VmMessage.AddWarningMessage(pr.messages, "Tracking number is missing!");
                else
                {
                    pr.payMaineRequest.TrackingInfo.TrackingNbr = (int)TrackingNbr;
                    bal.ValidateTrkNr(pr);
                }
            }
            catch (Exception ex)
            {
                Utilities.Error.logError(MethodBase.GetCurrentMethod().ToString(), ex);
                vm.VmMessage.AddErrorMessage(pr.messages, ex.Message);
            }
            string json = nt.JsonConvert.SerializeObject(pr);
            ViewBag.json = json;
            return View();
        }

        [HttpPost]
        [ValidateApiAntiForgeryTokenAttribute]
        public ActionResult PostData(vm.Vm_PayMaineRequestCollection pmc)
        {
            try
            {
                var pmr = pmc.payMaineRequest;
                pmc.messages.Clear();
                if (pmr.PaymentMethod == Utilities.Globals.creditCardPayment || pmr.PaymentMethod == Utilities.Globals.ACHPayment) { }
                else
                {
                    vm.VmMessage.AddErrorMessage(pmc.messages, "Please choose a payment method");
                }
                if (pmc.messages.Count == 0)
                {
                    var balpr = new bal.Bal_PaymentRequest();
                    balpr.Post(pmc);
                }
            }
            catch (Exception ex)
            {
                Utilities.Error.logError(MethodBase.GetCurrentMethod().ToString(), ex);
                vm.VmMessage.AddErrorMessage(pmc.messages, ex.Message);
            }
            return Json(pmc, JsonRequestBehavior.AllowGet);
        }
    }
}