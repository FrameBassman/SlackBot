namespace SetWalletBot.Core.Configuration.Middleware
{
    using System.Collections.Generic;
    using Noobot.Core.Configuration;
    using Noobot.Core.MessagingPipeline.Middleware;
    using Noobot.Core.MessagingPipeline.Middleware.ValidHandles;
    using Noobot.Core.MessagingPipeline.Request;
    using Noobot.Core.MessagingPipeline.Response;
    using Noobot.Core.Plugins.StandardPlugins;
    
    public class JiraMiddleware : MiddlewareBase
    {
        private readonly IConfigReader _configReader;
        private readonly StatsPlugin _statsPlugin;

        /// <summary>
        /// Requires config entry 'jira:prefix', 'jira:username' and 'jira:password' to be populated
        /// </summary>
        public JiraMiddleware(IMiddleware next, IConfigReader configReader, StatsPlugin statsPlugin) : base(next)
        {
            _configReader = configReader;
            _statsPlugin = statsPlugin;

            string prefix = _configReader.GetConfigEntry<string>("jira:prefix");
            
            HandlerMappings = new[]
            {
                new HandlerMapping
                {
                    ValidHandles = StartsWithHandle.For(prefix),
                    Description = "Gets information about Jira ticket - usage: @{bot} @{ticket number}",
                    EvaluatorFunc = JiraHandler,
                }
            };
        }

        private IEnumerable<ResponseMessage> JiraHandler(IncomingMessage arg1, IValidHandle arg2)
        {
            throw new System.NotImplementedException();
        }
    }
}