using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using utlg = LUPC.Utilities.Globals;

namespace LUPC.ViewModels
{
        public class VmMessage
        {
            public string status { get; set; }  // Warning, Error, etc.
            public string content { get; set; }

            static public void AddWarningMessage(List<VmMessage> msgList, string content)
            {
                msgList.Add(new VmMessage { status = utlg.msgWarning, content = content });
            }

            static public void AddErrorMessage(List<VmMessage> msgList, string content)
            {
                msgList.Add(new VmMessage { status = utlg.msgDanger, content = content });
            }

            static public void AddSuccessMessage(List<VmMessage> msgList, string content)
            {
                msgList.Add(new VmMessage { status = utlg.msgSuccess, content = content });
            }

            static public void FinalPostMessage(List<VmMessage> msgList, bool success, int fieldErrorCount)
            {
                if (success)
                    msgList.Add(new VmMessage { status = utlg.msgSuccess, content = "Update Successful" });
                else if (fieldErrorCount > 0)
                    msgList.Add(new VmMessage { status = utlg.msgDanger, content = "See below for field specific errors" });
            }
        }
}