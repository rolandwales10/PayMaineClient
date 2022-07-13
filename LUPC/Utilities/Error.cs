using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

/*
 *  Synopsis: Logs errors.  This is most relevant for database detected errors, where the error
 *      message is in the exception, but not always in the same place within it.
 *  
 *  October 2016  Roland Wales
 *  
 *  Change log:
 */


namespace LUPC.Utilities
{
    public class Error
    {
        public static void logError(string referenceLocation, Exception exParm)
        {
            /*
             *  Write the database error message to the log
             */

            string msg = referenceLocation;
            try {
                /*
                 *  Look for database detected errors first
                 */
                //if (exParm.InnerException != null && exParm.InnerException.InnerException != null)
                //{
                //    msg += exParm.InnerException.InnerException.ToString();
                //}
                if (exParm.Message != null)
                    msg += exParm.Message;
                else
                    msg += "undetermined error message";
                Logging.writeLogError(msg);
                if (exParm.StackTrace != null)
                    Logging.writeLogError(exParm.StackTrace);
                if (exParm.InnerException != null)
                    getInnerExceptions(exParm.InnerException);
            }
            catch (Exception)
            {
                string st = exParm.Message;   /* no where to log this error, use this for debugging */
            }
        }

        public static void getInnerExceptions(object comServer)
        {
            string exception = "";

            exception = (((Exception)comServer).Message);
            Logging.writeLogError(exception);
            if (((Exception)comServer).InnerException != null)
            {
                getInnerExceptions(((Exception)comServer).InnerException);
            }
        }

        public static List<string> modelErrorsToList(ModelStateDictionary ModelState)
        {
            /*
             * Capture model errors to send to the client and display to the user
             */
            var errors = new List<string>();
            var erroneousFields = ModelState.Where(ms => ms.Value.Errors.Any())
                                            .Select(x => new { x.Key, x.Value.Errors });

            foreach (var erroneousField in erroneousFields)
            {
                var fieldError = erroneousField.Errors
                                   .Select(r => r.ErrorMessage);
                errors.AddRange(fieldError);
            }
            return errors;
        }

    }
}
