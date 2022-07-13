using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUPC.Utilities
{
    public class Globals
    {
        public const string billerId = "LUP";
        public const string applicationId = "LUPCPayments";
        public const string cash = "Cash";
        public const string creditCardPayment = "CC";
        public const string ACHPayment = "ACH";
        public const string parmDept = "dept";
        public const string parmPaymentMethod = "PaymentMethod";
        public const string parmClientPaymentId = "ClientPaymentId";

        public const string statusSubmitted = "Submitted";
        public const string statusPaid = "Paid";
        public const string statusNotPaid = "Not Paid";
        public const string cancelled = "Cancelled";
        public const string declined = "Declined";

        // https://www.tutorialrepublic.com/twitter-bootstrap-tutorial/bootstrap-alerts.php
        public const string msgDanger = "danger";
        public const string msgInfo = "info";
        public const string msgSuccess = "success";
        public const string msgWarning = "warning";

        public static string databaseError = "Database update error - please contact application support";
    }
}