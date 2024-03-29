﻿using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace DebugHelper
{
    public class Debugger
    {
        private const string Header = "[KInfoBoard]: ";
        public static void WriteDebugLog(string message)
        {
            Debug.WriteLine(Header + message); //TODO: create log file? or sending it to App Insights?
        }

#pragma warning disable CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        public static async void WriteErrorLog(string message, Exception ex)
        {
            WriteDebugLog(Header + $"Caught an exception {ex}({ex.HResult}). Message: {message}");
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
#pragma warning restore CS1998

        public static async Task ShowErrorDialog(string message, Exception ex)
        {
            WriteErrorLog(message, ex);
            var dialog = new MessageDialog($"Caught an exception {ex}({ex.HResult}). Message: {ex.Message}", message);
            await dialog.ShowAsync();
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
