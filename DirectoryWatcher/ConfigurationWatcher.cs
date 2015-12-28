using System.Threading;

namespace DirectoryWatcher
{
    internal class ConfigurationWatcher
    {
        private Debug Debug;
        private CountdownEvent Countdown;

        public ConfigurationWatcher(Debug debug)
        {
            Debug = debug;
        }

        public void Start(CountdownEvent countdown, string path)
        {
            Countdown = countdown;
            ThreadPool.QueueUserWorkItem((state) => Run(path));
        }

        private void Run(string path)
        {
            Debug.WriteLine("Starting configuration watcher");
        }

        public void Stop()
        {
            Debug.WriteLine("Stopping configuration watcher");
            Countdown.Signal();
        }
    }
}