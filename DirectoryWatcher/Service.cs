using System;
using System.Collections.Generic;
using System.Threading;

namespace DirectoryWatcher
{
    internal class Service
    {
        public event EventHandler ConfigurationChanged;
        public event EventHandler SourcesChanged;

        private Debug Debug;
        private Commander Commander;
        private ConfigurationWatcher ConfigurationWatcher;
        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        private List<Worker> Workers { get; } = new List<Worker>();

        public Service(Debug debug, string configurationPath)
        {
            Debug = debug;

            Workers.Add(Commander = new Commander(this, "Commander"));
            Workers.Add(ConfigurationWatcher = new ConfigurationWatcher(this, "Configuration Watcher", configurationPath));
        }

        public void Run()
        {
            using (var countdown = new CountdownEvent(Workers.Count))
            {
                Commander.Commands.Add("test", () => Debug.WriteLine("test"));
                Commander.Shutdown += (sender, e) => Shutdown();

                foreach (var worker in Workers)
                    worker.Start(countdown, CancellationTokenSource.Token);

                countdown.Wait();
                Debug.WriteLine("END");
            }
        }

        public void Shutdown()
        {
            Debug.WriteLine("Shutdown requested...");
            CancellationTokenSource.Cancel();
        }
    }
}