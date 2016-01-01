using System;
using System.IO;
using System.Security.Cryptography;
using Timer = System.Timers.Timer;

namespace DirectoryWatcher
{
    internal class FileWatcher : IDisposable
    {
        private FileInfo File;
        private FileSystemWatcher Watcher;
        private byte[] Hash;
        private Timer NotFoundTimer;

        public event EventHandler Changed;

        public event EventHandler Removed;

        public FileWatcher(string path)
        {
            File = new FileInfo(path);
            File.Refresh();
            File = new FileInfo(path);
            if (!File.Exists)
                throw new FileNotFoundException(string.Format("File `{0}` not found", File.FullName), File.FullName);

            Watcher = new FileSystemWatcher(File.DirectoryName, File.Name);
            Watcher.EnableRaisingEvents = true;

            Watcher.Created += HandleChange;
            Watcher.Changed += HandleChange;
            Watcher.Deleted += HandleChange;
            Watcher.Renamed += HandleChange;

            Hash = GetFileHash(File);

            NotFoundTimer = new Timer(100);
            NotFoundTimer.Elapsed += (sender, e) =>
                Removed?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Watcher?.Dispose();
                NotFoundTimer?.Dispose();
            }
        }

        private void HandleChange(object sender, FileSystemEventArgs e)
        {
            File.Refresh();

            if (!File.Exists)
            {
                if (!NotFoundTimer.Enabled)
                    NotFoundTimer.Start();
                return;
            }

            if (NotFoundTimer.Enabled)
                NotFoundTimer.Stop();

            if (!CheckChanged(File, ref Hash))
                return;

            Changed?.Invoke(this, EventArgs.Empty);
        }

        private bool CheckChanged(FileInfo file, ref byte[] oldHash)
        {
            var hash = GetFileHash(File);
            for (var i = 0; i < hash.Length; i++)
                if (hash[i] != oldHash[i])
                {
                    oldHash = hash;

                    return true;
                }

            oldHash = hash;

            return false;
        }

        private byte[] GetFileHash(FileInfo file)
        {
            using (var md5 = MD5.Create())
            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                byte[] bytes = new byte[content.Length * sizeof(char)];
                Buffer.BlockCopy(content.ToCharArray(), 0, bytes, 0, bytes.Length);

                return md5.ComputeHash(bytes);
            }
        }
    }
}