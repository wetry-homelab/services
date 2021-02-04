using System;

namespace Application.Contracts.Response
{
    public class ClusterItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
    }
}
