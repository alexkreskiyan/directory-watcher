using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DirectoryWatcher
{
    internal class Configuration
    {
        public List<string> Include { get; set; }

        public List<Regex> IncludeRegex { get; set; }

        public List<string> Exclude { get; set; }

        public List<Regex> Enclude { get; set; }
    }
}