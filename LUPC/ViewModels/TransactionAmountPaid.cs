using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUPC.ViewModels
{
    public class TransactionStatus
    {
        public TransactionStatus()
        {
            AmountPaid = 0;
            ErrorMsg = "";
        }
        public string ClientApplicationId { get { return Utilities.Globals.applicationId; } }
        public string ClientPaymentId { get; set; }
        public decimal? AmountPaid { get; set; }
        public string Status { get; set; }
        public string ErrorMsg { get; set; }
    }
}