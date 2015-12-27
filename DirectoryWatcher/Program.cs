using System;
using System.Threading;

namespace DirectoryWatcher
{
    internal class Program
    {
        private static Commander Commander = new Commander();

        /*
        Usage:
        - project.json describes:
        -- explicitly included files
        -- pattern-based includes
        -- explicitly excluded files
        -- pattern-based excludes
        Workflow:
        - program loads project.json
        - program sends notification objects with changed/removed/added files' names
        -- simply on each change
        -- on submit command
        1. commander, that can be assigned with commands
        2. configuration watcher, that observes configuration changes and manages watcher rules
        3. watcher, that observes changes according to provided rules and has suspend/resume commands for output buffering
        - removed sources
        - added sources
        - changed sources
        - renamed sources
        permissions are monitored too:
        - if file becomes available it is considered as added one
        - if file becomes unavailable it is considered as removed one
        */

        private static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            Debug.WriteLine("Booting up...");

            Console.CancelKeyPress += Shutdown;
            Run();

            Debug.WriteLine("End...");
        }

        private static void Run()
        {
            Commander.Start();
        }

        private static void Shutdown(object sender, ConsoleCancelEventArgs e)
        {
            Debug.WriteLine("Shutdown requested...");
            Commander.Stop();
            e.Cancel = true;
        }
    }
}