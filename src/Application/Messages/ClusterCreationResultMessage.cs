using System.Text.Json.Serialization;

namespace Application.Messages
{
    public class ClusterCreationResultMessage
    {
        [JsonPropertyName("pattern")]
        public string Pattern { get; set; }
        [JsonPropertyName("data")]
        public ClusterData Data { get; set; }
    }

    public class ClusterData
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }
        [JsonPropertyName("kubeConfig")]
        public string KubeConfig { get; set; }
        [JsonPropertyName("kubeConfigAsJson")]
        public string KubeConfigAsJson { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
