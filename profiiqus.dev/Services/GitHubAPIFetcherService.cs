using Newtonsoft.Json;
using Octokit;
using profiiqus.dev.Models;
using System.Configuration;
using System.Net;

namespace profiiqus.dev.Services
{
    public class GitHubAPIFetcherService
    {

        private Dictionary<String, double> languageStats;
        private CancellationToken languageStatsToken;
        private Credentials credentials;
        private GitHubClient client;
        private IConfiguration configuration;

        public GitHubAPIFetcherService(IConfiguration configuration) {
            this.configuration = configuration;

            this.languageStats = new Dictionary<String, double>();
            this.languageStatsToken = new CancellationToken();

            this.credentials = new Credentials(configuration["Application:GitHubToken"]);
            this.client = new GitHubClient(new ProductHeaderValue(configuration["Application:ProductName"]))
            {
                Credentials = credentials
            };

            FetchLanguageStats();
            ScheduleDataRefresh(languageStatsToken);
        }

        public Dictionary<String, double> GetLanguageStats()
        {
            return languageStats;
        }

        private async void ScheduleDataRefresh(CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    await Task.Delay(60000, cancellationToken);
                    FetchLanguageStats();
                }
            });
        }

        private async void FetchLanguageStats()
        {
            try
            {
                Dictionary<string, double> languageBytes = new Dictionary<string, double>();
                var repos = client.Repository.GetAllForUser(configuration["Application:GitHubUsername"]).Result;
                foreach(var repo in repos)
                {
                    if (repo.Fork) continue; 

                    IReadOnlyList<RepositoryLanguage>? languages = null;
                    Task.Run(async () => { languages = await client.Repository.GetAllLanguages(repo.Id); }).Wait();

                    foreach (RepositoryLanguage lan in languages)
                    {
                        if(!languageBytes.ContainsKey(lan.Name))
                        {
                            languageBytes[lan.Name] = (double) lan.NumberOfBytes;
                        } else
                        {
                            languageBytes[lan.Name] = languageBytes[lan.Name] + (double)lan.NumberOfBytes;
                        }
                    }
                }

                int sumBytes = SumBytes(languageBytes);
                foreach (KeyValuePair<string, double> kvp in languageBytes)
                {
                    languageStats[kvp.Key] = kvp.Value / (double)sumBytes * 100;
                }

                languageStats = (from entry in languageStats orderby entry.Value descending select entry).ToDictionary<KeyValuePair<String, double>, String, double>(pair => pair.Key, pair => pair.Value);

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private int SumBytes(Dictionary<string, double> languageBytes)
        {
            var sum = 0;
            foreach(KeyValuePair<string, double> kvp in languageBytes)
            {
                sum += (int)kvp.Value;
            }
            return sum;
        }
    }
}
