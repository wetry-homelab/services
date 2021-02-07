using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.Messages
{
    public class ClusterCreateMessage
    {
        [JsonPropertyName("pattern")]
        public string Pattern { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Ssh
    {
        [JsonPropertyName("publicKey")]
        public string PublicKey { get; set; }

        [JsonPropertyName("privateKey")]
        public string PrivateKey { get; set; }
    }

    public class Config
    {
        [JsonPropertyName("user")]
        public string User { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("ssh")]
        public Ssh Ssh { get; set; }
    }

    public class Node
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("disk")]
        public int Disk { get; set; }

        [JsonPropertyName("memory")]
        public int Memory { get; set; }

        [JsonPropertyName("cpuCores")]
        public int CpuCores { get; set; }

        [JsonPropertyName("template")]
        public string Template { get; set; }

        [JsonPropertyName("master")]
        public bool Master { get; set; }

        [JsonPropertyName("proxmoxNode")]
        public string ProxmoxNode { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("config")]
        public Config Config { get; set; }

        [JsonPropertyName("nodes")]
        public List<Node> Nodes { get; set; }
    }
}