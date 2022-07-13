using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using bal = LUPC.BusinessAreaLayer;
using nt = Newtonsoft.Json;
using mdl = LUPC.Models;
using vm = LUPC.ViewModels;
using utl = LUPC.Utilities;
using System.Configuration;
using System.Net;
using System.IO;
using System.Reflection;

namespace LUPC.BusinessAreaLayer
{
    public class Bal_TrackingNbr
    {
        public mdl.Entities db = null;
        public UnitOfWork uow = null;
        public Bal_TrackingNbr()
        {
            uow = new UnitOfWork();
            db = uow.db;
        }
        public void ValidateTrkNr(vm.Vm_PayMaineRequestCollection pr)
        {
            /*
             * Validates a tracking number for eligibility to be paid.  Out of convenience, this also returns information
             * about the tracking number.
             */
            const string recentCheckout = "This tracking number has a very recent Checkout and payment is pending.  Please wait 1 hour and try again.  If it has not been paid at that time, you will be able to pay it then.";
            pr.messages.Clear();
            var pmr = pr.payMaineRequest;
            var ti = pmr.TrackingInfo;
            if (ti.TrackingNbr > 0)
            {
                int trkNr = ti.TrackingNbr;
                var action = db.Action.Where(r => r.Action_ID == trkNr).FirstOrDefault();
                if (action == null)
                {
                    vm.VmMessage.AddWarningMessage(pr.messages, "Tracking number not found");
                    ti.TrackingNbr = 0;   // Clear out any previous results
                    ti.StaffMember = "";
                    return;
                }
                else
                {
                    var sc = db.Action.Include(r => r.LU_Action_Number_Type).Where(r => r.Action_ID == trkNr)
                        .Select(r => r.LU_Action_Number_Type.Code).FirstOrDefault();
                    if (sc == null)
                    {
                        vm.VmMessage.AddWarningMessage(pr.messages, "Tracking number needs an action code before it can be paid");
                        return;
                    }
                }

                if (action.Action_Fee == null || action.Action_Fee <= 0)
                {
                    vm.VmMessage.AddWarningMessage(pr.messages, "No fee has been established for this application."
                        + "  Please check your Tracking Number provided to you by LUPC staff.");
                    return;
                }
                int checkRecordId = GetCost(ti);
                if (checkRecordId > 0)
                {
                    var ckr = db.Check_Record.Where(r => r.Check_Record_ID == checkRecordId).FirstOrDefault();
                    if (ckr != null)
                    {
                        {
                            /*
                             * Get closure on the transaction.  If a transaction is in a submitted status after
                             * an hour, find out if it has been paid and update the status.  The results won't 
                             * affect the status of this method call, but are needed to get the status of the
                             * payment up to date.
                             */
                            var TxnAmt = new vm.TransactionStatus();
                            TxnAmt.ClientPaymentId = ckr.Check_Record_ID.ToString();
                            var balap = new bal.Bal_ConfirmAmountPaid();
                            balap.GetAmount(TxnAmt, pr.messages);
                            if (TxnAmt.AmountPaid == null)
                                vm.VmMessage.AddWarningMessage(pr.messages, recentCheckout);

                            ti.BalanceDueD -= TxnAmt.AmountPaid ?? 0;
                        }
                    }
                }
                if (ti.BalanceDueD <= 0)
                {
                    vm.VmMessage.AddWarningMessage(pr.messages, "This application fee has been paid.  Enter another tracking number.");
                    return;
                }
            }
            else vm.VmMessage.AddErrorMessage(pr.messages, "Tracking number missing from the request");
        }

        public void GetData(vm.Vm_PayMaineRequestCollection pr)
        {
            /*
             * Provide data to confirm that the tracking number is the right one to be paid
             * 
             */
            pr.messages.Clear();
            var pmr = pr.payMaineRequest;
            var ti = pmr.TrackingInfo;
            if (ti.TrackingNbr > 0)
            {
                var trkNr = ti.TrackingNbr;
                var action = db.Action.Where(r => r.Action_ID == trkNr).FirstOrDefault();
                if (action == null)
                {
                    vm.VmMessage.AddWarningMessage(pr.messages, "Tracking number not found");
                    ti.TrackingNbr = 0;   // Clear out any previous results
                    ti.StaffMember = "";
                    return;
                }

                if (action.Action_Fee == null)
                {
                    vm.VmMessage.AddWarningMessage(pr.messages, "Error: Goat Action record has no fee");
                    return;
                }
                ti.StaffMember = db.Staff.Where(r => r.Staff_ID == action.Staff_ID).Select(r => r.Name).FirstOrDefault();
                ti.Town = db.Action_Town.Include(r => r.Town).Where(r => r.Action_ID == trkNr)
                        .Select(r => r.Town.Name).FirstOrDefault();
                ti.NatureOfProposal = action.Description;

                if (action.LU_Action_Number_Type == null && action.Permit_Year == null && action.Action_Number_Sequence == null && action.Permit_Amendment == null)
                    ti.ApplicationTypeNbr = action.Action_ID.ToString();
                else
                {
                    var actionTypeCode = db.LU_Action_Number_Type.Where(r => r.LU_Action_Number_Type_ID == action.LU_Action_Number_Type_ID)
                        .Select(r => r.Code).FirstOrDefault();
                    ti.ApplicationTypeNbr = actionTypeCode + "-";
                    switch (actionTypeCode)
                    {
                        case "AR":
                        case "LOE":
                        case "EC":
                        case "SD":
                        case "BLN":
                        case "GN":
                            ti.ApplicationTypeNbr += action.Permit_Year;
                            break;
                        default:
                            break;
                    }
                    ti.ApplicationTypeNbr += action.Action_Number_Sequence;
                    if (action.Permit_Amendment != null && action.Permit_Amendment != "1")
                        ti.ApplicationTypeNbr += action.Permit_Amendment;
                }
                GetCost(ti);

                var nameKeys = db.Action_Name.Where(r => r.Action_ID == action.Action_ID).Select(r => r.Person_ID).ToList();
                ti.People = new List<string>();
                foreach (var nameK in nameKeys)
                {
                        ti.People.Add(db.Person.Where(r => r.Person_ID == nameK).Select(r => r.Name).FirstOrDefault());
                }
            }
            else vm.VmMessage.AddWarningMessage(pr.messages, "Error: Tacking number is missing");
        }

        public int GetCost(vm.Vm_TrackingNbrInfo ti)
        {
            const int CocFeeId = 17;    //  This is the lu_fee_id for source code COC in the lu_fee table
            int IdInSubmittedStatus = 0;
            int trkNr = ti.TrackingNbr;
            decimal amountPaid = 0;
            decimal COCAmoutPaid = 0;

            ti.TotalFeeD = db.Action.Where(r => r.Action_ID == trkNr).Select(r => r.Action_Fee).FirstOrDefault() ?? 0M;

            var checkr = db.Check_Record.Where(r => r.Action_ID == trkNr).ToList();
            foreach (var item in checkr)
            {
                if (item.Date_Deposit != null)
                {
                    if (item.LU_Fee_ID == CocFeeId)
                        COCAmoutPaid += (decimal)item.Amount;
                    else
                        amountPaid += (decimal)item.Amount;
                }
                else
                {
                    if (item.Status == utl.Globals.statusSubmitted)
                        IdInSubmittedStatus = item.Check_Record_ID;
                }
            }

            ti.BalanceDueD = ti.TotalFeeD - amountPaid;
            if (ti.BalanceDueD == 0)
            {
                /*
                 * Find out if there is a balance due for certificates of completion
                 */
                ti.TotalFeeD = db.COC.Where(r => r.Action_ID == trkNr).Select(r => r.Fee).Sum() ?? 0;
                ti.BalanceDueD = ti.TotalFeeD - COCAmoutPaid;
                if (ti.BalanceDueD > 0)
                    ti.BalanceDueIsCertificatesOfCompletion = true;
            }

            ti.TotalFee = string.Format("{0:C}", ti.TotalFeeD);
            /*
             * Record the balance due only if no payments are in submitted, or payment pending status.
             * Payment pending status must be resolved prior to making payments.
             */
            if (IdInSubmittedStatus == 0)
                ti.BalanceDue = string.Format("{0:C}", ti.BalanceDueD);

            return IdInSubmittedStatus;
        }
    }
}
