namespace Infrastructure.Contracts.Response
{
    public class DatacenterNodeResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Ip { get; set; }

        public bool Online { get; set; }

        public string PveVersion { get; set; }

        public string KernelVersion { get; set; }

        public int Uptime { get; set; }

        public double Mhz { get; set; }

        public string Model { get; set; }

        public bool Hvm { get; set; }

        public int Core { get; set; }

        public int UserHz { get; set; }

        public int Socket { get; set; }

        public string Flag { get; set; }

        public int Thread { get; set; }

        public long RootFsUsed { get; set; }

        public long RootFsTotal { get; set; }

        public long RootFsFree { get; set; }

        public long RootFsAvailable { get; set; }

        public long RamTotal { get; set; }

        public long RamFree { get; set; }

        public long SwapUsed { get; set; }

        public long SwapTotal { get; set; }

        public long SwapFree { get; set; }

        public long RamUsed { get; set; }
    }
}
