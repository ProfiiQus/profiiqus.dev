using profiiqus.dev.Services;

namespace profiiqus.dev.Models
{
    public class IndexModel
    {
        private readonly IConfiguration configuration;
        private GitHubAPIFetcherService service;

        public IndexModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.service = new GitHubAPIFetcherService(configuration);
        }

        public Dictionary<String, double> GetGitHubLanguageStats()
        {
            return service.GetLanguageStats();
        }

        public int GetYearsSinceBirthday()
        {
            var zeroTime = new DateTime(1, 1, 1);
            var birthday = DateTime.Parse(configuration["Application:Birthday"]);
            var today = DateTime.Now;

            TimeSpan span = today - birthday;
            int years = (zeroTime + span).Year - 1;
            return years;
        }
    }
}
