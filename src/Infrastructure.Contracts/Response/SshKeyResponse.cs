namespace Infrastructure.Contracts.Response
{
    public class SshKeyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Fingerprint { get; set; }
    }
}
