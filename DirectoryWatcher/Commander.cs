using System;
using System.Collections.Generic;
using System.Threading;

namespace DirectoryWatcher
{
    internal class Commander
    {
        public Dictionary<string, Action> Commands { get; } = new Dictionary<string, Action>();

        public event EventHandler Shutdown;

        private Debug Debug;
        private CountdownEvent Countdown;

        public Commander(Debug debug)
        {
            Debug = debug;
        }

        public void Start(CountdownEvent countdown)
        {
            Countdown = countdown;
            ThreadPool.QueueUserWorkItem(Run);
        }

        private void Run(object state)
        {
            Debug.WriteLine("Starting commander");

            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                Shutdown?.Invoke(this, e);
                Countdown.Signal();
            };

            var commandName = Console.ReadLine();
            while (commandName != null)
            {
                SendCommand(commandName);
                commandName = Console.ReadLine();
            }
        }

        private void SendCommand(string name)
        {
            if (!Commands.ContainsKey(name))
            {
                Debug.WriteLine("Unknown command `{0}`", name);
                return;
            }

            Debug.WriteLine("Run command `{0}`", name);
            Commands[name]?.Invoke();
        }
    }
}