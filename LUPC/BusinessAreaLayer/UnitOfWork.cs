using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
// using mdle = LUPC.Models;
using mdl = LUPC.Models;
using mvc = System.Web.Mvc;
using vm = LUPC.ViewModels;
using utl = LUPC.Utilities;

/*
 *  This class manages the creation of instances of dbset, and provides a method to save changes.
 *  It can be used to commit changes from one or more entities as a single transaction.
 */

namespace LUPC.BusinessAreaLayer
{
    public class UnitOfWork : IDisposable
    {
        public mdl.Entities db = new mdl.Entities();

        public UnitOfWork()
        {
        }

        public void Save(string locationIdentifier, List<vm.VmMessage> messages)
        {
            try
            {
                bool saveFailed;
                do
                {
                    saveFailed = false;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        // Update the values of the entity that failed to save from the store
                        ex.Entries.Single().Reload();
                    }

                } while (saveFailed);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var item in ex.EntityValidationErrors)
                {
                    foreach (var it in item.ValidationErrors)
                    {
                        //  When the message pertains to a property (field in the table), the property name is included in the text
                        vm.VmMessage.AddWarningMessage(messages, it.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LUPC.Utilities.Error.logError(locationIdentifier, ex);
                vm.VmMessage.AddErrorMessage(messages, utl.Globals.databaseError);

            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
