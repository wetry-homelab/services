using System.Text.Json.Serialization;

namespace Infrastructure.Contracts.Response
{
    public class KubeconfigDownloadResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
