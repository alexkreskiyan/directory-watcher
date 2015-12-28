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

            if (Shutdown != null)
                Shutdown(this, EventArgs.Empty);

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

            var command = Console.ReadLine();
            while (command != null)
            {
                SendCommand(command);
                command = Console.ReadLine();
            }
        }

        private void SendCommand(string command)
        {
            if (!Commands.ContainsKey(command))
            {
                Debug.WriteLine("Unknown command `{0}`", command);
                return;
            }

            Debug.WriteLine("Run command `{0}`", command);
            Commands[command]();
        }
    }
}