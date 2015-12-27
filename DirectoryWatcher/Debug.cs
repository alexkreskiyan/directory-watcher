using System;
using System.Diagnostics;
using System.Threading;

namespace DirectoryWatcher
{
    internal class Debug
    {
        [Conditional("DEBUG")]
        public static void WriteLine(string message)
        {
            Write(message);
        }

        [Conditional("DEBUG")]
        public static void WriteLine(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        private static void Write(string message)
        {
            Console.Write(DateTime.Now.ToString("HH:mm:ss.ffff "));
            Console.Write("{0,-15} ", string.Concat('[', Thread.CurrentThread.Name, ']'));
            Console.WriteLine(message);
        }
    }
}