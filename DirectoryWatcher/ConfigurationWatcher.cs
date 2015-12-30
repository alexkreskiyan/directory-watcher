using System.Threading;

namespace DirectoryWatcher
{
    internal class ConfigurationWatcher : Worker
    {
        private string Path;

        public ConfigurationWatcher(Service service, string name, string path)
            : base(service, name)
        {
            Path = path;
        }

        protected override void Run(CancellationToken token)
        {
            using (var watcher = new FileWatcher(Path))
            {
                watcher.Changed += (sender, e) => Debug.WriteLine("Configuration file changed");
                watcher.Removed += (sender, e) =>
                {
                    Debug.WriteLine("Configuration file removed");
                    Service.Shutdown();
                };
                token.WaitHandle.WaitOne();
                Debug.WriteLine("Shutting down configuration watcher");
            }
        }
    }
}