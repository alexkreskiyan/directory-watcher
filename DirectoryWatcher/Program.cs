using System.Threading;

namespace DirectoryWatcher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            Debug.WriteLine("Booting up...");

            //shutdown if no arguments
            if (args.Length == 0)
            {
                Debug.WriteLine("Please, provide configuration file path");
                return;
            }

            new Service().Run(args[0]);
        }
    }
}

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