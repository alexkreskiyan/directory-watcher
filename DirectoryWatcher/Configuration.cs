using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DirectoryWatcher
{
    internal class Configuration
    {
        public List<string> Include { get; set; }

        public List<Regex> IncludeRegex { get; set; }

        public List<string> Exclude { get; set; }

        public List<Regex> ExcludeRegex { get; set; }

        public string Main { get; set; }
    }

    internal class RawConfiguration
    {
        public static explicit operator Configuration(RawConfiguration raw)
        {
            var configuration = new Configuration();

            configuration.Include = raw.include.ToList();
            configuration.IncludeRegex = raw.includeRegex.Select((value) => new Regex(value)).ToList();
            configuration.Exclude = raw.exclude.ToList();
            configuration.ExcludeRegex = raw.excludeRegex.Select((value) => new Regex(value)).ToList();

            return configuration;
        }

        public List<string> include { get; set; }

        public List<string> includeRegex { get; set; }

        public List<string> exclude { get; set; }

        public List<string> excludeRegex { get; set; }

        public string Main { get; set; }
    }
}