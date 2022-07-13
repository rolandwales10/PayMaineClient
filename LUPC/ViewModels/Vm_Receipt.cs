using System;
using System.Collections.Generic;
using System.Configuration;

namespace LUPC.ViewModels
{
    public class Vm_Receipt
    {
        public Vm_Receipt()
        {
        }
        public int ClientPaymentId { get; set; } 
        public string Status { get; set; }
        public string LUPCEmail { get { return ConfigurationManager.AppSettings["LUPCEmail"]; } }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateStr { get; set; }
        public string TrackingNbr { get; set; }
        public string Name { get; set; }
        public string deptName { get; set; }
        public string ContactInfo { get; set; }
        public string ContactEmail { get; set; }
        public string ErrorMsg { get; set; }
        public string price { get; set; }
        public string fee { get; set; }
        public string total { get; set; }
        public decimal totalAmtDec { get; set; }
        public string comments { get; set; }
    }
    public class Vm_ReceiptCollection
    {
        public Vm_ReceiptCollection()
        {
            receipt = new Vm_Receipt();
            messages = new List<VmMessage>();
        }
        public Vm_Receipt receipt { get; set; }
        public List<VmMessage> messages { get; set; }
    }
}