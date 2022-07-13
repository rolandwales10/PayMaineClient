using System.Linq;
using System.Data.Entity;
using ml = System.Net.Mail;
using mdl = LUPC.Models;
using System;
using System.Configuration;
using System.Reflection;

namespace LUPC.BusinessAreaLayer
{
    public class Bal_Send_Mail
    {
        mdl.Entities db = new mdl.Entities();
        public void FeePaid(string clientPaymentId)
        {
            int clientPaymentInt = Convert.ToInt32(clientPaymentId);
            var LUPCEmail = ConfigurationManager.AppSettings["LUPCEmail"];
            var ckr = db.Check_Record.Where(r => r.Check_Record_ID == clientPaymentInt).FirstOrDefault();
            if (ckr != null)
            {
                    var msg = new ml.MailMessage();
                    var env = ConfigurationManager.AppSettings["Environment"];
                    var subjectTrailer = "";
                    if (env == "Prod")
                        subjectTrailer = "";
                    else subjectTrailer = " (" + env + ")";
                    if (string.IsNullOrEmpty(ckr.Email)) { }
                    else addRecipient(msg,ckr.Email);
                    if (LUPCEmail.Length > 0)
                        addRecipient(msg,LUPCEmail);
                    var ActionFields = db.Action.Where(r => r.Action_ID == ckr.Action_ID).Select(r => new { r.Region_ID, r.Staff_ID}).FirstOrDefault();
                    if (ActionFields != null)
                    {
                        /*
                         * Send mail to both the LURC office and the staff member
                         */
                        string officeEmail = "";
                        //if (ActionFields.LURC_OFFICE != null)
                        //{
                        //    officeEmail = db.LURC_REG_LU.Where(r => r.LURC_OFF == ActionFields.LURC_OFFICE).Select(r => r.PermitEmail).FirstOrDefault();
                        //    if (string.IsNullOrEmpty(officeEmail)) { }
                        //    else addRecipient(msg, officeEmail);
                        //}
                        if (ActionFields.Staff_ID != null)
                        {
                            var staffEmail = db.Staff.Where(r => r.Staff_ID == ActionFields.Staff_ID).Select(r => r.Name).FirstOrDefault();
                            if (!string.IsNullOrEmpty(staffEmail))
                            {
                                staffEmail += "@maine.gov";
                                if (staffEmail != (officeEmail ?? ""))
                                    addRecipient(msg, staffEmail);
                            }
                        }
                    }
                    if (msg.To.Count > 0)
                    {
                        msg.Subject = "LUPC Application fee paid for " + ckr.Action_ID + subjectTrailer;
                        if (!string.IsNullOrEmpty(LUPCEmail))
                            msg.From = new ml.MailAddress(LUPCEmail);

                        msg.IsBodyHtml = true;
                        msg.Body = "<h1>" + msg.Subject + "</h1>"
                                + "<br>"
                                + "Transaction Date: " + ckr.Date_Deposit.ToString() + "<br>"
                                + "Tracking Number: " + ckr.Action_ID + "<br>"
                                + "Payer: " + ckr.Check_Name + "<br>"
                                + "<br>"
                                + "<table>"
                                + "<tr>"
                                + "<th width='20%' style='text-align:right'>Application Fee</th>"
                                + "<th width='20%' style='text-align:right'>Application Transaction Fee</th>"
                                + "<th width='20%' style='text-align:right'>Total</th>"
                                + "</tr>"
                                + "<tr>"
                                + "<td style='text-align:right'>" + string.Format("{0:C}", ckr.Amount) + "</td>"
                                + "<td style='text-align:right'>" + string.Format("{0:C}", ckr.Application_Transaction_Fee) + "</td>"
                                + "<td style='text-align:right'>" + string.Format("{0:C}", ckr.Amount + ckr.Application_Transaction_Fee) + "</td>"
                                + "</tr>"
                                + "</table>"
                                + "<br>"
                                + "If you have questions or concerns, please call " + ConfigurationManager.AppSettings["Phone#"];
                        ml.SmtpClient smtp = new ml.SmtpClient("smtp.state.me.us");
                        smtp.Send(msg);
                    }
                    return;
                }

            Utilities.Logging.writeLogError("Error: send mail called with check record id " + clientPaymentId + ", which is missing data");
        }

        public void addRecipient(ml.MailMessage msg, string address)
        {
            try
            {
                msg.To.Add(address);
            }
            catch (Exception ex)
            {
                Utilities.Error.logError(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}