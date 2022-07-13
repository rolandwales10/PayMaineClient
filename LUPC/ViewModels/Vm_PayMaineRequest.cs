using System;
using System.Collections.Generic;
using utl = LUPC.Utilities;

namespace LUPC.ViewModels
{
    public class Vm_PayMaineRequestItem
    {
        public string ProductId { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get { return 1; } }
        public string ReferenceInformation { get; set; }
    }
    public class Vm_PayMaineRequest
    {
        public Vm_PayMaineRequest()
        {
            PaymentItems = new List<Vm_PayMaineRequestItem>();
            PaymentItems.Add(new Vm_PayMaineRequestItem() );
            TrackingInfo = new Vm_TrackingNbrInfo();
        }
        public string ClientApplicationId { get { return utl.Globals.applicationId; } }
        public int ClientPaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string Comments { get; set; }

        public List<Vm_PayMaineRequestItem> PaymentItems { get; set; }
        public Vm_TrackingNbrInfo TrackingInfo { get; set; }
    }

    public class Vm_PayMaineRequestCollection
    {
        public Vm_PayMaineRequestCollection()
        {
            payMaineRequest = new Vm_PayMaineRequest();
            messages = new List<VmMessage>();
        }
        public Vm_PayMaineRequest payMaineRequest { get; set; }
        public List<VmMessage> messages { get; set; }
    }
}