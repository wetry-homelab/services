using System.Text.Json.Serialization;

namespace Infrastructure.Contracts.Response
{
    public class DatacenterNodeResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("online")]
        public bool Online { get; set; }

        [JsonPropertyName("pveVersion")]
        public string PveVersion { get; set; }

        [JsonPropertyName("kernelVersion")]
        public string KernelVersion { get; set; }

        [JsonPropertyName("uptime")]
        public int Uptime { get; set; }

        [JsonPropertyName("mhz")]
        public double Mhz { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("hvm")]
        public bool Hvm { get; set; }

        [JsonPropertyName("core")]
        public int Core { get; set; }

        [JsonPropertyName("userHz")]
        public int UserHz { get; set; }

        [JsonPropertyName("socket")]
        public int Socket { get; set; }

        [JsonPropertyName("flag")]
        public string Flag { get; set; }

        [JsonPropertyName("thread")]
        public int Thread { get; set; }

        [JsonPropertyName("rootFsUsed")]
        public long RootFsUsed { get; set; }

        [JsonPropertyName("rootFsTotal")]
        public long RootFsTotal { get; set; }

        [JsonPropertyName("rootFsFree")]
        public long RootFsFree { get; set; }

        [JsonPropertyName("rootFsAvailable")]
        public long RootFsAvailable { get; set; }

        [JsonPropertyName("ramTotal")]
        public long RamTotal { get; set; }

        [JsonPropertyName("ramFree")]
        public long RamFree { get; set; }

        [JsonPropertyName("swapUsed")]
        public long SwapUsed { get; set; }

        [JsonPropertyName("swapTotal")]
        public long SwapTotal { get; set; }

        [JsonPropertyName("swapFree")]
        public long SwapFree { get; set; }

        [JsonPropertyName("ramUsed")]
        public long RamUsed { get; set; }
    }
}
