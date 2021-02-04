namespace Application.Contracts.Response
{
    public class ClusterDetailsResponse : ClusterItemResponse
    {
        public string Description { get; set; }
        public int Node { get; set; }
        public int Cpu { get; set; }
        public int Memory { get; set; }
        public int Storage { get; set; }
        public string Ip { get; set; }
    }
}
