namespace SetWalletBot.Toolbox.Middleware
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Linq;
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
        /// Requires config entry 'jira:prefix', 'jira:url' and 'jira:base64Token' to be populated
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
                    ValidHandles = ContainsTextHandle.For(prefix),
                    Description = "Gets information about Jira ticket",
                    EvaluatorFunc = JiraHandler,
                }
            };
        }

        private IEnumerable<ResponseMessage> JiraHandler(IncomingMessage message, IValidHandle matchedHandle)
        {
            string searchTerm = message.TargetedText.Substring(matchedHandle.HandleHelpText.Length).Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                yield return message.ReplyToChannel($"Please give me something to search, e.g. {matchedHandle} trains");
            }
            else
            {
                yield return message.IndicateTypingOnChannel();
                string url = _configReader.GetConfigEntry<string>("jira:url");
                string prefix = _configReader.GetConfigEntry<string>("jira:prefix");
                string base64Token = _configReader.GetConfigEntry<string>("jira:base64Token");

                if (string.IsNullOrEmpty(url) && string.IsNullOrEmpty(prefix) && string.IsNullOrEmpty(base64Token))
                {
                    _statsPlugin.IncrementState("Jira:Failed");
                    yield return message.ReplyToChannel("Woops, looks like settings for Jira is not configured properly. Please ask the admin to fix this");
                }
                else
                {
                    string messageToAnswer = GetDataFromJira(message.RawText, url, prefix, base64Token);
                    yield return message.ReplyToChannel(messageToAnswer);
                }
            }
        }

        public static string GetDataFromJira(string m, string jiraUrl, string prefix, string base64Token)
        {
            var result = "";
            var issue = ParseMentionedIssue(m, prefix);
            var url = $"{jiraUrl}/rest/api/2/issue/{issue}";

            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Authorization, $"Basic {base64Token}");
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            using (StreamReader sr = new StreamReader(webClient.OpenRead(url)))
            {
                var json = JObject.Parse(sr.ReadToEnd());
                var summary = json["fields"]["summary"].Value<string>();
                var status = json["fields"]["status"]["name"].Value<string>();
                var assegnee = json["fields"]["assignee"]["displayName"].Value<string>();
                result = new StringBuilder()
                    .AppendLine($"Issue {jiraUrl}/browse/{issue.ToUpper()}")
                    .Append("Title: ").AppendLine(summary)
                    .Append("Status: ").AppendLine(status)
                    .Append("Assignee: ").Append(assegnee)
                    .ToString();
            }

            return result;
        }

        public static string ParseMentionedIssue(string m, string prefix)
        {
            string regexPart = "";
            foreach (var letter in prefix)
            {
                regexPart += "[" + letter.ToString().ToUpper() + letter.ToString().ToLower() + "]";
            }
            
            Regex regex = new Regex(regexPart + "-[0-9]*");
            return regex.Match(m).Value;
        }
        
    }
}