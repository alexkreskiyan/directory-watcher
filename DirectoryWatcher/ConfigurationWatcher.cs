using Newtonsoft.Json;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading;

namespace DirectoryWatcher
{
    internal class ConfigurationWatcher : Worker
    {
        public event EventHandler ConfigurationChanged;
        public event EventHandler SourceRulesChanged;
        private FileInfo File;
        private RawConfiguration Configuration;

        public ConfigurationWatcher(Service service, string name, string path)
            : base(service, name)
        {
            File = new FileInfo(path);
        }

        protected override void Run(CancellationToken token)
        {
            using (var watcher = new FileWatcher(File.FullName))
            {
                Configuration = GetConfiguration();

                var observable = Observable.FromEventPattern(
                    handler => watcher.Changed += handler,
                    handler => watcher.Changed -= handler
                )
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(e =>
                {
                    Debug.WriteLine("Configuration file changed");
                    HandleUpdate();
                });

                watcher.Removed += (sender, e) =>
                {
                    Debug.WriteLine("Configuration file removed");
                    Service.Shutdown();
                };

                //wait until cancelled
                token.WaitHandle.WaitOne();
                Debug.WriteLine("Shutting down configuration watcher");
                observable.Dispose();
            }
        }

        private void HandleUpdate()
        {
            //get actual configuration
            var configuration = GetConfiguration();
        }

        private RawConfiguration GetConfiguration()
        {
            using (var stream = File.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<RawConfiguration>(content, new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Error,
                    NullValueHandling = NullValueHandling.Include
                });
            }
        }
    }

    internal class ConfigurationUpdateEventArgs
    {

    }
}