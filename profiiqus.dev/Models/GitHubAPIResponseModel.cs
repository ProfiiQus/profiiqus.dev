using Newtonsoft.Json;

namespace profiiqus.dev.Models
{
    public class GitHubAPIResponseModel
    {
        [JsonProperty("languages_url")]
        public string LanguagesURL { get; set; }

    }
}
