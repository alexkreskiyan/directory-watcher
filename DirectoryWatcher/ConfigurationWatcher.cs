using System.IO;
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
            Debug.WriteLine("Running configuration watcher");

            if (!File.Exists(Path))
                throw new FileNotFoundException(string.Format("Project configuration file `{0}` not found", Path), Path);

            Debug.WriteLine("Starting configuration watcher");
            Thread.Sleep(10000);
        }
    }
}