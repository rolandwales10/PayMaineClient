using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using mdl = LUPC.Models;

/*
 *  Synopsis: Writes log messages.
 *  
 *  October 2016  Roland Wales
 *  
 *  Change log:
 */
 
namespace LUPC.Utilities
{
    public static class Logging
    {
        static mdl.Entities db = new mdl.Entities();
        public static void writeLogError(string logMsg)
        {
            int rowsAffected;
            string nowStr = DateTime.Now.ToString();
            try
            {
                rowsAffected = db.Database.ExecuteSqlCommand("insert into dbo.event_Log (eventDate, eventMsg)  values (@create_time, @log_message)",
                    new SqlParameter("@create_time", nowStr), new SqlParameter("@log_message", logMsg));
            }
            catch (Exception) { }    // No place to log the error!
        }

        public static void writeLogInfo(string logMsg)
        {
            writeLogError(logMsg);
        }
    }
}