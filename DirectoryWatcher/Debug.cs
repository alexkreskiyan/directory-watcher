using System;
using System.Diagnostics;
using System.Threading;

namespace DirectoryWatcher
{
    internal class Debug
    {
        private object consoleLocker = new object();

        [Conditional("DEBUG")]
        public void WriteLine(string message)
        {
            Write(message);
        }

        [Conditional("DEBUG")]
        public void WriteLine(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        private void Write(string message)
        {
            lock (consoleLocker)
            {
                Console.Write(DateTime.Now.ToString("HH:mm:ss.ffff "));
                Console.Write("{0,-15} ", string.Concat('[', Thread.CurrentThread.Name, ']'));
                Console.WriteLine(message);
            }
        }
    }
}