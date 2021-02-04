namespace Infrastructure.Contracts.Request
{
    public class SshKeyCreateRequest
    {
        public string Name { get; set; }
        public bool AutoGenerate { get; set; }
        public string Public { get; set; }
    }
}
