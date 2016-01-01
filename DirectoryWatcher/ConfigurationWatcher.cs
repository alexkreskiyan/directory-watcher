using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace DirectoryWatcher
{
    internal class ConfigurationWatcher : Worker
    {
        private FileInfo File;

        public ConfigurationWatcher(Service service, string name, string path)
            : base(service, name)
        {
            File = new FileInfo(path);
        }

        protected override void Run(CancellationToken token)
        {
            using (var watcher = new FileWatcher(File.FullName))
            {
                LogFile();
                watcher.Changed += (sender, e) =>
                {
                    Debug.WriteLine("Configuration file changed");
                    LogFile();
                };
                watcher.Removed += (sender, e) =>
                {
                    Debug.WriteLine("Configuration file removed");
                    Service.Shutdown();
                };
                token.WaitHandle.WaitOne();
                Debug.WriteLine("Shutting down configuration watcher");
            }
        }

        private void LogFile()
        {
            using (var stream = File.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                var configuration = JsonConvert.DeserializeObject<Configuration>(content);
                if (configuration != null)
                {
                    Debug.WriteLine("Include:{0}", configuration.Include?.Count);
                    Debug.WriteLine("IncludeRegex:{0}", configuration.IncludeRegex?.Count);
                    Debug.WriteLine("Exclude:{0}", configuration.Exclude?.Count);
                    Debug.WriteLine("ExcludeRegex:{0}", configuration.ExcludeRegex?.Count);
                }
            }
        }
    }
}