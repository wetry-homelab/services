using System.Text.Json.Serialization;

namespace Infrastructure.Contracts.Response
{
    public class TemplateResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("baseTemplate")]
        public string BaseTemplate { get; set; }

        [JsonPropertyName("cpuCount")]
        public int CpuCount { get; set; } = 1;

        [JsonPropertyName("memoryCount")]
        public int MemoryCount { get; set; } = 1024;

        [JsonPropertyName("diskSpace")]
        public int DiskSpace { get; set; } = 20;
    }
}
