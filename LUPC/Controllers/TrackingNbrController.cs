using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using nt = Newtonsoft.Json;
using bal = LUPC.BusinessAreaLayer;
using vm = LUPC.ViewModels;
using System.Configuration;

namespace LUPC.Controllers
{
    public class TrackingNbrController : Controller
    {
        // GET: TrackingNbr
        public ActionResult Select()
        {
            /*
             * Stop the user if offline.txt exists with a message in it.  This is only done on the home page,
             * allowing users to complete a transaction if they're not on the home page.
             */
            FileInfo file;
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/offline.txt");
            string offlineMsg = "";

            file = new FileInfo(filename);
            if (file.Exists)
            {
                foreach (string line in System.IO.File.ReadLines(file.FullName))
                {
                    offlineMsg += line + "  ";
                }
                if (offlineMsg.Length > 0)
                    return Content(offlineMsg);
            }

            var env = ConfigurationManager.AppSettings["Environment"];
            if (env == "Prod")
                ViewBag.Environment = "";       // Don't display production, only other environments
            else ViewBag.Environment = env + " Environment";
            return View();
        }

        public ActionResult Validate(int? TrackingNbr)
        {
            /*
             *  This is called by the client right after selecting a tracking number.
             *  If it passes validation the next step is showing the confirmation screen.
             */
            var pr = new vm.Vm_PayMaineRequestCollection();
            try
            {
                var bal = new bal.Bal_TrackingNbr();
                if (TrackingNbr == null)
                    vm.VmMessage.AddWarningMessage(pr.messages, "Please enter a tracking number");
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
            return Json(pr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Confirm(int? TrackingNbr)
        {
            var pr = new vm.Vm_PayMaineRequestCollection();
            try
            {
                var bal = new bal.Bal_TrackingNbr();
                if (TrackingNbr == null)
                    vm.VmMessage.AddWarningMessage(pr.messages, "Please enter a tracking number");
                else
                {
                    pr.payMaineRequest.TrackingInfo.TrackingNbr = (int)TrackingNbr;
                    bal.GetData(pr);
                    if (pr.payMaineRequest.TrackingInfo.BalanceDueIsCertificatesOfCompletion)
                        vm.VmMessage.AddSuccessMessage(pr.messages, "Balance due is for one or more certificates of completion");
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
    }
}