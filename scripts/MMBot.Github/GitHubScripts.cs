using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MMBot.Adapters;
using MMBot.Scripts;
using Octokit;
using Octokit.Internal;

namespace MMBot.Github
{
    public class GitHubScripts : IMMBotScript
    {
        private string _username;
        private GitHubClient _client;
        private string _password;

        public void Register(Robot robot)
        {
            _username = robot.GetConfigVariable("MMBOT_GITHUB_USERNAME");
            _password = robot.GetConfigVariable("MMBOT_GITHUB_PASSWORD");
            _client = !string.IsNullOrEmpty(_username) 
                ? new GitHubClient(new ProductHeaderValue("mmbot"), new InMemoryCredentialStore(new Credentials(_username, _password))) 
                : new GitHubClient(new ProductHeaderValue("mmbot"));
            
            robot.Respond(@"github show me the last (\d)+ pushes on (.*)[/\\](.*)", async msg =>
            {
                var count = msg.Match[1];
                var user = msg.Match[2];
                var repo = msg.Match[3];
                var events = await _client.;

                var i = events.Count();
            });
        }

        public IEnumerable<string> GetHelp()
        {
            return new string[]
            {

            };
        }
    }
}
