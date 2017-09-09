using Noobot.Core.Configuration;
using SetWalletBot.Toolbox.Middleware;

namespace SetWalletBot.Presentation.Slack.Configuration
{
    public class ExampleConfiguration : ConfigurationBase
    {
        public ExampleConfiguration()
        {
            UseMiddleware<JiraMiddleware>();
        }
    }
}