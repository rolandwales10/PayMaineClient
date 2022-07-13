using System.Configuration;
using System.IO;
using System.Net;
using mdl = LUPC.Models;
using nt = Newtonsoft.Json;
using vm = LUPC.ViewModels;

namespace LUPC.BusinessAreaLayer
{
    public class Bal_Payment
    {
        public mdl.Entities db = null;
        public UnitOfWork uow = null;
        public Bal_Payment()
        {
            uow = new UnitOfWork();
            db = uow.db;
        }

        /*
         * Create the Elavon payment request.  The controller will redirect to the bank's payment screen after this.
         */
        public void Checkout(vm.Vm_PayMaineRequestCollection pmc)
        {
            var pmr = pmc.payMaineRequest;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string tokenRequestMessages = "";

            string url = ConfigurationManager.AppSettings["PayMaine_Request_Begin"];
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var balpr = new Bal_PaymentRequest();
            balpr.Get(pmr);
            pmr.TrackingInfo = null;    // Not needed for Pay Maine
            string output = nt.JsonConvert.SerializeObject(pmr);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(output);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                tokenRequestMessages = streamReader.ReadToEnd();
            }
            var response = nt.JsonConvert.DeserializeObject<vm.Vm_NewPaymentResponse>(tokenRequestMessages);
            foreach (var item in response.messages)
            {
                vm.VmMessage.AddErrorMessage(pmc.messages, item);
            }
        }
    }
}
