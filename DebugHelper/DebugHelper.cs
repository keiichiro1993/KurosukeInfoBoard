using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
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

        public static async void WriteErrorLog(string message, Exception ex)
        {
            WriteDebugLog(Header + "Caught an exception " + ex.HResult + ". Message: " + message);
            printError(ex);

#if !DEBUG
            if (await AppCenter.IsEnabledAsync())
            {
                var properties = new Dictionary<string, string>
                {
                    { "CustomMessage", message }
                };
                Crashes.TrackError(ex, properties);
            }
#endif
        }

        private static void printError(Exception ex)
        {
            WriteDebugLog(ex.Message);
            WriteDebugLog(ex.StackTrace);
            if (ex.InnerException != null)
            {
                WriteDebugLog("==== in ====");
                printError(ex.InnerException);
            }
        }
    }
}
