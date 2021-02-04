using System;
using System.Text.Json.Serialization;

namespace Application.Messages
{
    public class ClusterUpdateMessage
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("node")]
        public int Node { get; set; }

        [JsonPropertyName("cpu")]
        public int Cpu { get; set; }

        [JsonPropertyName("memory")]
        public int Memory { get; set; }

        [JsonPropertyName("storage")]
        public string Storage { get; set; }

        [JsonPropertyName("isUpdate")]
        public bool IsUpdate { get; set; } = true;
    }
}
