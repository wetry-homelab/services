using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Cluster
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int Node { get; set; } = 2;

        [Required]
        public int Cpu { get; set; } = 1;

        [Required]
        public int Memory { get; set; } = 512;

        [Required]
        public int Storage { get; set; } = 20;

        [Required]
        public string Ip { get; set; }

        public string KubeConfig { get; set; }

        [Required]
        public string SshKey { get; set; }

        [Required]
        public string User { get; set; } = "root";

        [Required]
        public string State { get; set; } = "Provisionning";

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeleteAt { get; set; }
        public virtual ICollection<ClusterNode> Nodes { get; set; }
    }
}
