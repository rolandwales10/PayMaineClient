using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using bal = LUPC.BusinessAreaLayer;
using nt = Newtonsoft.Json;
using vm = LUPC.ViewModels;

namespace LUPC.Controllers
{
    public class PaymentResponseController : Controller
    {
        // GET: PaymentResponse
        // PayMaine routes to this on success
        public ActionResult Success(string ClientApplicationId, string ClientPaymentId)
        {
            /*
             * This get method is open to tampering.  All a user has to do is enter the right URL string to make it look like they paid.
             * The API call will confirm that the amount invoiced has been fully paid prior to issuing a receipt.
             */
            string message = "";
            int ClientPaymentInt;
            var rctc = new vm.Vm_ReceiptCollection();
            var rct = rctc.receipt;
            try
            {
                var balrc = new bal.Bal_Receipt();
                Int32.TryParse(ClientPaymentId, out ClientPaymentInt);
                rct.ClientPaymentId = Convert.ToInt32(ClientPaymentId);
                balrc.Get(rctc);

                if (rctc.messages.Count == 0)
                {
                    var diff = DateTime.Now - rct.TransactionDate;
                    if (diff.TotalMinutes > 15)
                        message = "Time expired for reviewing receipt";
                    else
                    {
                        if (rctc.receipt.Status == Utilities.Globals.statusSubmitted)
                        {
                            vm.TransactionStatus TxnAmt = new vm.TransactionStatus();
                            TxnAmt.ClientPaymentId = ClientPaymentId;
                            /*
                             * Do an API call to PayMaine to confirm the amount paid
                             */
                            var balap = new bal.Bal_ConfirmAmountPaid();
                            balap.GetAmount(TxnAmt, rctc.messages);

                            if (TxnAmt.ErrorMsg != null && TxnAmt.ErrorMsg.Length > 0)
                                message = TxnAmt.ErrorMsg;
                            else
                            {
                                if (TxnAmt.Status != Utilities.Globals.statusPaid)
                                {
                                    message = "This tracking number has gone through check out recently and has not been paid yet.";
                                }
                            }
                            /*
                             * Send email if the receipt is being processed right after a payment.
                             * If it has already been paid (see below) don't resend the email.
                             */
                            if (message.Length == 0 && TxnAmt.Status == Utilities.Globals.statusPaid)
                            {
                                var balm = new bal.Bal_Send_Mail();
                                balm.FeePaid(ClientPaymentId);
                            }
                        }
                        //  Already paid prior to this display of a receipt
                        else if (rctc.receipt.Status == Utilities.Globals.statusPaid) { }
                        else
                        {
                            message = "This tracking number has not been paid";
                        }
                    }
                }
                else message = rctc.messages[0].content;
            }
            catch (Exception ex)
            {
                Utilities.Error.logError(MethodBase.GetCurrentMethod().ToString(), ex);
                message = ex.Message;
            }
            ViewBag.message = message;
            return View(rct);
        }

        public ActionResult Declined(string ClientApplicationId, string ClientPaymentId, string ErrorMsg)
        {
            try
            {
                var balps = new bal.Bal_PaymentStatus();
                int checkRecordId = Convert.ToInt32(ClientPaymentId);
                balps.Update(checkRecordId, Utilities.Globals.declined);
            }
            catch (Exception ex)
            {
                Utilities.Error.logError(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            ViewBag.ErrorMsg = ErrorMsg;
            return View();
        }

        public ActionResult Cancelled(string ClientApplicationId, string ClientPaymentId, string ErrorMsg)
        {
            try
            {
                var balps = new bal.Bal_PaymentStatus();
                int checkRecordId = Convert.ToInt32(ClientPaymentId);
                balps.Update(checkRecordId, Utilities.Globals.cancelled);
            }
            catch (Exception ex)
            {
                Utilities.Error.logError(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            ViewBag.ErrorMsg = ErrorMsg;
            return View();
        }
    }
}