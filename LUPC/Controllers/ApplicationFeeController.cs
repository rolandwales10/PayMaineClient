using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LUPC.Controllers
{
    public class ApplicationFeeController : Controller
    {
        // The published URL for this app has this entry point.  It simply redirects to the beginning screen for making payments.
        public ActionResult Pay()
        {
            //return RedirectToAction("Select", "TrackingNbr");
            return View();
        }
    }
}