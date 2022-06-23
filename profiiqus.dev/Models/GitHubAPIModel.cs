using profiiqus.dev.Services;

namespace profiiqus.dev.Models
{
    public class GitHubAPIModel
    {
        private readonly IConfiguration configuration;
        private GitHubAPIFetcherService service;

        public GitHubAPIModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.service = new GitHubAPIFetcherService(configuration);
        }

        public Dictionary<String, double> GetLanguageStats()
        {
            return service.GetLanguageStats();
        }
    }
}
