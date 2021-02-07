namespace Infrastructure.Contracts.Request
{
    public class ClusterCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Node { get; set; }
        public int Cpu { get; set; }
        public int Memory { get; set; }
        public int Storage { get; set; }
        public int? SelectedTemplate { get; set; }
        public string SshKey { get; set; }
    }
}
