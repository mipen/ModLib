using System;
using System.Runtime.InteropServices;

namespace ModLib.Definitions.Debugging
{
    public static class ModDebug
    {
        private static Delegate _showError = null;
        private static Delegate _showMessage = null;
        private static Delegate _logError = null;

        [DllImport(AssemblyChecker.AssemblyNameFull)]
        public static extern void ShowError(string message, string title = "", Exception exception = null);

        public static void ShowMessage(string message, string title = "", bool nonModal = false)
        {

        }

        public static void LogError(string error, Exception ex = null)
        {

        }
    }
}
