using System;
using System.Collections.Generic;
using System.Threading;

namespace DirectoryWatcher
{
    internal class Commander
    {
        private bool Active;

        public Dictionary<string, Action> Commands { get; } = new Dictionary<string, Action>();

        public void Start()
        {
            Debug.WriteLine("Starting commander");

            Active = true;
            while (Active)
            {
                SendCommand(Console.ReadLine());
                Thread.Sleep(50);
            }
        }

        public void Stop()
        {
            Debug.WriteLine("Stopping commander");

            Active = false;
        }

        private void SendCommand(string command)
        {
            if (command == null)
                return;

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