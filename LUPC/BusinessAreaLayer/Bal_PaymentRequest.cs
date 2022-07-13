using System.Linq;
using System;
using System.Data.Entity;
using mdl = LUPC.Models;
using utl = LUPC.Utilities;
using vm = LUPC.ViewModels;
using System.Collections.Generic;
using System.Reflection;

namespace LUPC.BusinessAreaLayer
{
    public class Bal_PaymentRequest
    {
        public mdl.Entities db = null;
        public UnitOfWork uow = null;
        public Bal_PaymentRequest()
        {
            uow = new UnitOfWork();
            db = uow.db;
        }

        public void Post(vm.Vm_PayMaineRequestCollection pmc)
        {
            bool newCheckRecord = false;
            var pmr = pmc.payMaineRequest;
            var trkNr = pmc.payMaineRequest.TrackingInfo.TrackingNbr;
            mdl.Check_Record ckr;
            /*
             * Reuse an existing Check_Record if it hasn't been paid
             */
            ckr = db.Check_Record.Where(r => r.Action_ID == trkNr && r.Date_Deposit == null).FirstOrDefault();
            if (ckr == null)
            {
                ckr = new mdl.Check_Record();
                //var maxId = db.Check_Record.OrderByDescending(r => r.Check_Record_ID).Select(r => r.Check_Record_ID).FirstOrDefault();
                //ckr.Check_Record_ID = maxId + 1;
                newCheckRecord = true;
            }
            var action = db.Action.Where(r => r.Action_ID == trkNr).FirstOrDefault();
            if (action == null)
            {
                vm.VmMessage.AddErrorMessage(pmc.messages, "Error: action code " + trkNr + " not found in the Action table");
                return;
            }
            ckr.Action_ID = pmc.payMaineRequest.TrackingInfo.TrackingNbr;
            ckr.Check_Date = DateTime.Now;

            var baltr = new Bal_TrackingNbr();
            baltr.GetCost(pmc.payMaineRequest.TrackingInfo);
            ckr.Amount = pmc.payMaineRequest.TrackingInfo.BalanceDueD;

            if (pmc.payMaineRequest.TrackingInfo.BalanceDueIsCertificatesOfCompletion)
            {
                ckr.LU_Fee_ID = db.LU_Fee.Where(r => r.Source_Code == "COC").Select(r => r.LU_Fee_ID).FirstOrDefault();
            }
            else
            {
                var actionTypeCode = db.LU_Action_Number_Type.Where(r => r.LU_Action_Number_Type_ID == action.LU_Action_Number_Type_ID)
                    .Select(r => r.Code).FirstOrDefault();
                ckr.LU_Fee_ID = db.LU_Fee.Where(r => r.Source_Code == actionTypeCode).Select(r => r.LU_Fee_ID).FirstOrDefault();
            }
            ckr.Comment = pmr.Comments;
            ckr.Enter_Date = DateTime.Now;
            ckr.Check_Name = pmr.FirstName + " " + pmr.LastName;
            ckr.Payment_Type = pmr.PaymentMethod;

            if (newCheckRecord)
                db.Check_Record.Add(ckr);
            else
                db.Entry(ckr).State = System.Data.Entity.EntityState.Modified;

            if (pmc.messages.Count == 0)
            {
                uow.Save(MethodBase.GetCurrentMethod().ToString(), pmc.messages);
            }
            pmc.payMaineRequest.TrackingInfo.CheckRecordId = ckr.Check_Record_ID;

            if (pmr.PaymentMethod == utl.Globals.ACHPayment)
                ckr.Application_Transaction_Fee = 0.25M;
            else ckr.Application_Transaction_Fee = (decimal)ckr.Amount * .03M;
            ckr.Address = pmr.Address;
            ckr.Zip_Code = pmr.ZipCode;
            ckr.Email = pmr.Email;
            ckr.Status = utl.Globals.statusSubmitted;
            // ckrs.Email to do:
            if (newCheckRecord)
                db.Check_Record.Add(ckr);
            else
                db.Entry(ckr).State = System.Data.Entity.EntityState.Modified;

            if (pmc.messages.Count == 0)
            {
                uow.Save(MethodBase.GetCurrentMethod().ToString(), pmc.messages);
            }
        }

        // Used to get the request for PayMaine after the data has been persisted in the post method below
        public void Get(vm.Vm_PayMaineRequest pmr)
        {
            var balTr = new Bal_TrackingNbr();
            var ckr = db.Check_Record.Where(r => r.Check_Record_ID == pmr.ClientPaymentId).FirstOrDefault();
            if (ckr != null)
            {
                /*
                 * ClientApplicationId is a constant, set in the view model class
                 * ClientPaymentId is in the input
                 */
                pmr.PaymentMethod = ckr.Payment_Type;
                // pmr.CompanyName
                pmr.FirstName = ckr.First_Name;
                pmr.LastName = ckr.Last_Name;
                pmr.Address = ckr.Address;
                pmr.ZipCode = ckr.Zip_Code;
                var it = pmr.PaymentItems[0];

                var luFee = db.LU_Fee.Where(r => r.LU_Fee_ID == ckr.LU_Fee_ID).FirstOrDefault();
                var TownType = db.Action_Town.Include(r => r.Town).Where(r => r.Action_ID == ckr.Action_ID)
                        .Select(r => r.Town.Type).FirstOrDefault();
                it.ProductId = "LUP_";
                if (TownType == "Unorganized")
                    it.ProductId += luFee.MRS_UT;
                else it.ProductId += luFee.LUPC_TP;

                pmr.TrackingInfo.TrackingNbr = (int)ckr.Action_ID;
                balTr.GetCost(pmr.TrackingInfo);
                it.Amount = pmr.TrackingInfo.BalanceDueD;

                // Submit a line item for the electronic payment fee
                var applicationFeeIt = new vm.Vm_PayMaineRequestItem();
                applicationFeeIt.ProductId = "LUP_EFEE";
                applicationFeeIt.Amount = ckr.Application_Transaction_Fee ?? 0;
                pmr.PaymentItems.Add(applicationFeeIt);
            }
        }
    }
}