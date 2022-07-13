using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using nt = Newtonsoft.Json;
using mdl = LUPC.Models;
using utl = LUPC.Utilities;
using vm = LUPC.ViewModels;

namespace LUPC.BusinessAreaLayer
{
    public class Bal_ConfirmAmountPaid
    {
        public mdl.Entities db = null;
        public UnitOfWork uow = null;
        public Bal_ConfirmAmountPaid()
        {
            uow = new UnitOfWork();
            db = uow.db;
        }
        public void GetAmount(vm.TransactionStatus TxnAmt, List<vm.VmMessage> messages)
        {
            /*
             * Do an API call to PayMaine to confirm the amount paid
             */
            var result = "";
            string url = ConfigurationManager.AppSettings["PayMaine_Request_Confirm_Status"];
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string output = nt.JsonConvert.SerializeObject(TxnAmt);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(output);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            var TxnAmt2 = nt.JsonConvert.DeserializeObject<LUPC.ViewModels.TransactionStatus>(result);           
            if (!String.IsNullOrEmpty(TxnAmt2.ErrorMsg))
            {
                var msg = new vm.VmMessage();
                msg.status = utl.Globals.msgDanger;
                msg.content = TxnAmt2.ErrorMsg;
                messages.Add(msg);
                utl.Logging.writeLogError(msg.content);
                return;
            }
            TxnAmt.AmountPaid = TxnAmt2.AmountPaid;
            TxnAmt.Status = TxnAmt2.Status;
            int Check_Record_ID = Convert.ToInt32(TxnAmt.ClientPaymentId);
            var ckr = new mdl.Check_Record();
            ckr = db.Check_Record.Where(r => r.Check_Record_ID == Check_Record_ID).FirstOrDefault();
            if (ckr != null)
            {
                /*
                 *  No need to update if the amount paid is null.  This means its in a payment pending (submitted) status
                 */
                if (TxnAmt.AmountPaid != null)
                {
                    if (TxnAmt.AmountPaid >= ckr.Amount + ckr.Application_Transaction_Fee)
                    {
                        ckr.Status = utl.Globals.statusPaid;
                        ckr.Date_Deposit = DateTime.Now;
                        db.Entry(ckr).State = System.Data.Entity.EntityState.Modified;
                    }
                    else ckr.Status = utl.Globals.statusNotPaid;
                    db.Entry(ckr).State = System.Data.Entity.EntityState.Modified;
                    uow.Save("Confirm Amount", messages);
                }
                return;
            }
            //  If we're still here, there is a problem!
            var msg2 = new vm.VmMessage();
            msg2.status = utl.Globals.msgDanger;
            msg2.content = "Error: missing data for the following Check_Record: " + TxnAmt.ClientPaymentId;
            messages.Add(msg2);
            utl.Logging.writeLogError(msg2.content);
        }
    }
}