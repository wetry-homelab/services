namespace Infrastructure.Contracts.Request
{
    public class ClusterUpdateRequest
    {
        public int Node { get; set; }
        public int Cpu { get; set; }
        public int Memory { get; set; }
        public int Storage { get; set; }
    }
}
