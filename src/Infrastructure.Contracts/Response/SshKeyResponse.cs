using System.Text.Json.Serialization;

namespace Infrastructure.Contracts.Response
{
    public class SshKeyResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("fingerprint")]
        public string Fingerprint { get; set; }
    }
}
