using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mdl = LUPC.Models;
using utl = LUPC.Utilities;

namespace LUPC.BusinessAreaLayer
{
    public class Bal_PaymentStatus
    {
        public mdl.Entities db = null;
        public UnitOfWork uow = null;
        public Bal_PaymentStatus()
        {
            uow = new UnitOfWork();
            db = uow.db;
        }
        public void Update(int checkRecordId, string status)
        {
            if (checkRecordId > 0)
            {
                var chk = db.Check_Record.Where(r => r.Check_Record_ID == checkRecordId).FirstOrDefault();
                if(chk != null)
                {
                    chk.Status = status;
                    db.Entry(chk).State = System.Data.Entity.EntityState.Modified;
                    uow.Save("Status update",null);
                }
            }
        }
    }
}