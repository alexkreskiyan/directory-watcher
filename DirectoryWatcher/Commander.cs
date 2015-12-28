using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryWatcher
{
    internal class Commander : Worker
    {
        public Dictionary<string, Action> Commands { get; } = new Dictionary<string, Action>();

        public event EventHandler Shutdown;

        public Commander(Service service, string name)
            : base(service, name)
        { }

        protected override void Run(CancellationToken token)
        {
            Debug.WriteLine("Running commander");

            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                Debug.WriteLine("Notify shutdown");
                Shutdown?.Invoke(this, e);
            };

            Debug.WriteLine("Read console input async...");
            var commandName = ReadCommandAsync(token);
            while (commandName != null)
            {
                SendCommand(commandName);
                Debug.WriteLine("Read console input async...");
                commandName = ReadCommandAsync(token);
            }
        }

        private string ReadCommandAsync(CancellationToken token)
        {
            return Task.Run(() => Console.ReadLine(), token).Result;
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