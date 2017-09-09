namespace SetWalletBot.Core.Configuration
{
    using Noobot.Core.Configuration;
    using Middleware;
    
    public class ExampleConfiguration : ConfigurationBase
    {
        public ExampleConfiguration()
        {
            UseMiddleware<JiraMiddleware>();
        }
    }
}