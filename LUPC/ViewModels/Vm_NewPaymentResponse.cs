using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUPC.ViewModels
{
    public class Vm_NewPaymentResponse
    {
        public string token { get; set; }
        public List<string> messages { get; set; }
    }
}