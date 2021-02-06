using System.Text.Json.Serialization;

namespace Infrastructure.Contracts.Response
{
    public class SshKeyDownloadResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
