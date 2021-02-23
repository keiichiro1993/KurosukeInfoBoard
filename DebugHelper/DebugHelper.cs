using System;
using System.Diagnostics;

namespace DebugHelper
{
    public class Debugger
    {
        private const string Header = "[KInfoBoard]: ";
        public static void WriteDebugLog(string message)
        {
            Debug.WriteLine(Header + message); //TODO: create log file? or sending it to App Insights?
        }

        public static void WriteErrorLog(string message, Exception ex)
        {
            WriteDebugLog(Header + "Caught an exception " + ex.HResult + ". Message: " + message);
            writeError(ex);
        }

        private static void writeError(Exception ex)
        {
            WriteDebugLog(ex.Message);
            WriteDebugLog(ex.StackTrace);
            if (ex.InnerException != null)
            {
                WriteDebugLog("==== in ====");
                writeError(ex.InnerException);
            }
        }
    }
}
