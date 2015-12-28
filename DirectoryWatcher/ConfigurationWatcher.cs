using System.IO;

namespace DirectoryWatcher
{
    internal class ConfigurationWatcher
    {
        private bool Active;
        private Debug Debug;

        public ConfigurationWatcher(Debug debug)
        {
            Debug = debug;
        }

        public void Start(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(string.Format("Configuration file `{0}` doesn't exist", path), path);

            Debug.WriteLine("Starting configuration watcher");

            Active = true;
        }

        public void Stop()
        {
            Debug.WriteLine("Stopping configuration watcher");

            Active = false;
        }
    }
}