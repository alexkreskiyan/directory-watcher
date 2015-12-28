using System.IO;
using System.Threading;

namespace DirectoryWatcher
{
    internal class Service
    {
        private Debug Debug;
        private Commander Commander;
        private ConfigurationWatcher ConfigurationWatcher;

        public Service(Debug debug)
        {
            Debug = debug;
            Commander = new Commander(Debug);
            ConfigurationWatcher = new ConfigurationWatcher(Debug);
        }

        public void Run(string configurationPath)
        {
            if (!File.Exists(configurationPath))
            {
                Debug.WriteLine("Configuration file `{0}` doesn't exist", configurationPath);
                return;
            }

            using (var countdown = new CountdownEvent(2))
            {
                Commander.Commands.Add("test", () => Debug.WriteLine("test"));
                Commander.Shutdown += (sender, e) => Shutdown();
                Commander.Start(countdown);

                ConfigurationWatcher.Start(countdown, configurationPath);

                countdown.Wait();
            }
        }

        private void Shutdown()
        {
            Debug.WriteLine("Shutdown requested...");
            ConfigurationWatcher.Stop();
        }
    }
}