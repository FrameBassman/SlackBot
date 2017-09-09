using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Noobot.Core.Configuration;

namespace SetWalletBot.Core.Configuration
{
    public class CustomConfigReader : IConfigReader
    {
        private IConfigurationSection _configurationSection;
        private const string SLACKAPI_CONFIGVALUE = "slack:apiToken";
        
        public CustomConfigReader(IConfigurationSection configSection)
        {
            _configurationSection = configSection;
        }
        
        public T GetConfigEntry<T>(string entryName)
        {
            throw new NotImplementedException();
        }

        public string SlackApiKey => _configurationSection[SLACKAPI_CONFIGVALUE];
        public bool HelpEnabled { get; set; } = true;
        public bool StatsEnabled { get; set; } = true;
        public bool AboutEnabled { get; set; } = true;
    }
}