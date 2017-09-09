using Microsoft.Extensions.Configuration;
using Noobot.Core.Configuration;

namespace SetWalletBot.Presentation.Slack.Configuration
{
    public class CustomConfigReader : IConfigReader
    {
        private readonly IConfigurationSection _configurationSection;
        private const string SlackApiConfigValue = "slack:apiToken";
        
        public CustomConfigReader(IConfigurationSection configSection)
        {
            _configurationSection = configSection;
        }
        
        public T GetConfigEntry<T>(string entryName)
        {
            return _configurationSection.GetValue<T>(entryName);
        }

        public string SlackApiKey => GetConfigEntry<string>(SlackApiConfigValue);
        public bool HelpEnabled { get; set; } = true;
        public bool StatsEnabled { get; set; } = true;
        public bool AboutEnabled { get; set; } = true;
    }
}