using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUPC.ViewModels
{
    public class Vm_TrackingNbrInfo
    {
        public int TrackingNbr { get; set; }
        public int CheckRecordId { get; set; }
        public string StaffMember { get; set; }
        public List<string> People { get; set; }
        public string Town { get; set; }
        public string ApplicationTypeNbr { get; set; }
        public string NatureOfProposal { get; set; }
        public bool BalanceDueIsCertificatesOfCompletion { get; set; }
        public decimal BalanceDueD { get; set; }
        public string BalanceDue { get; set; }
        public decimal TotalFeeD { get; set; }
        public string TotalFee { get; set; }
    }
}