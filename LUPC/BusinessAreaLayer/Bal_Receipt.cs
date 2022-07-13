using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using mvc = System.Web.Mvc;
using mdl = LUPC.Models;
//using mdle = PayMaineEntryDataModel.Models;
using vm = LUPC.ViewModels;
using System.Configuration;
using System;

namespace LUPC.BusinessAreaLayer
{
    public class Bal_Receipt
    {
        UnitOfWork uow = null;
        mdl.Entities db = null;
        //mdle.TreasuryEntitiesE db = null;

        public Bal_Receipt()
        {
            uow = new UnitOfWork();
            db = uow.db;
        }

        public void Get(vm.Vm_ReceiptCollection pmc)
        {
            var receipt = pmc.receipt;
            DateTime dt = DateTime.Now;
            var ckr = db.Check_Record.Where(r => r.Check_Record_ID == receipt.ClientPaymentId).FirstOrDefault();
            if (ckr != null)
            {
                pmc.receipt.TrackingNbr = ckr.Action_ID.ToString();
                receipt.Status = ckr.Status;
                if (ckr.Check_Date != null)
                {
                    dt = (DateTime)ckr.Check_Date;
                    receipt.TransactionDateStr = dt.ToString("MM/dd/yyyy");
                    receipt.TransactionDate = dt;
                }
                receipt.Name = ckr.Check_Name;

                receipt.ContactInfo = "If you have questions or concerns, please call " + ConfigurationManager.AppSettings["Phone#"];
                receipt.price = string.Format("{0:C}", ckr.Amount);
                receipt.fee = string.Format("{0:C}", ckr.Application_Transaction_Fee);
                receipt.totalAmtDec = (decimal)ckr.Amount + (decimal)(ckr.Application_Transaction_Fee ?? 0);
                receipt.total = string.Format("{0:C}", receipt.totalAmtDec);
                receipt.comments = ckr.Comment;
            }
        }
    }
}
