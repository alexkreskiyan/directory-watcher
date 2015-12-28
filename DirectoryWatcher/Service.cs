using System.Threading;

namespace DirectoryWatcher
{
    internal class Service
    {
        private Commander Commander = new Commander();
        private ConfigurationWatcher ConfigurationWatcher = new ConfigurationWatcher();

        public void Run(string configurationPath)
        {
            using (var countdown = new CountdownEvent(1))
            {
                Commander.Commands.Add("test", () => Debug.WriteLine("test"));
                Commander.Shutdown += (sender, e) => Shutdown();
                Commander.Start(countdown);

                ConfigurationWatcher.Start(configurationPath);

                countdown.Wait();
            }
        }

        private void Shutdown()
        {
            Debug.WriteLine("Shutdown requested...");
        }
    }
}