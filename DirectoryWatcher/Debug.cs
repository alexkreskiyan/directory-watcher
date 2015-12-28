using System;
using System.Diagnostics;
using System.Text;

namespace DirectoryWatcher
{
    internal class Debug
    {
        public string Name { get; }

        public Debug(string name)
        {
            Name = name;
        }

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
            var builder = new StringBuilder(DateTime.Now.ToString("HH:mm:ss.ffff "));
            builder.Append(string.Format("{0,-15} ", string.Concat('[', Name, ']')));
            builder.Append(message);
            Console.WriteLine(builder.ToString());
        }
    }
}