using System;
using System.Collections.Generic;
using System.Threading;

namespace DirectoryWatcher
{
    internal class Commander
    {
        public Dictionary<string, Action> Commands { get; } = new Dictionary<string, Action>();

        public event EventHandler Shutdown;

        private CountdownEvent Countdown;

        private void HandleShutdown(object sender, ConsoleCancelEventArgs e)
        {
            Debug.WriteLine("Handle Shutdown event");

            e.Cancel = true;

            Shutdown?.Invoke(this, e);

            Countdown.Signal();
        }

        public void Start(CountdownEvent countdown)
        {
            Countdown = countdown;
            ThreadPool.QueueUserWorkItem(Run);
        }

        private void Run(object state)
        {
            Debug.WriteLine("Starting commander");

            Console.CancelKeyPress += HandleShutdown;

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