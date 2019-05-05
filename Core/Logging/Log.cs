using System;
using System.Diagnostics;

namespace HomeCTRL.Backend.Core.Logging 
{
    public class Log
    {
        public static void Info(string tag, string message)
        {
            // TODO implement properly
            Console.WriteLine(string.Format("[{0}]\t{1}", tag, message));
        }

        public static void Error(string tag, string message)
        {
            // TODO implement properly
            Console.WriteLine(string.Format("[{0}]\t{1}", tag, message));
            Debug.WriteLine(string.Format("[{0}]\t{1}", tag, message));
        }
    }
}